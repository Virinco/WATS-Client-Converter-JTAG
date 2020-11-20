using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Virinco.WATS.Interface;



namespace JTAGConverters
{
    public class XJLogConverter : IReportConverter_v2
    {
        Dictionary<string, string> parameters;
        public Dictionary<string, string> ConverterParameters => parameters;

        public void CleanUp()
        {            
        }


        /// <summary>
        /// Arguments to converter, set when configuring in WATS Client
        /// </summary>
        public XJLogConverter()
        {
            parameters = new Dictionary<string, string>()
            {
                {"partNumber","PN"},
                {"partRevision","1.0" },
                {"operationTypeCode","210" }, 
                {"sequenceName", "JTAGSeq1" }, //Maybe xjlink-name?
                {"sequenceVersion","1.0.0" } 
            };
        }

        public XJLogConverter(Dictionary<string, string> args)
        {
            parameters = args;
        }


        public Report ImportReport(TDM api, Stream file)
        {
            XDocument infoXml;
            using (ZipFile zipFile = new ZipFile(file))
            {
                ZipEntry zipEntry = zipFile.GetEntry("Info.xml");
                using (Stream zipStream = zipFile.GetInputStream(zipEntry))
                {
                    using (StreamReader streamReader = new StreamReader(zipStream))
                    {
                        infoXml = XDocument.Load(streamReader);
                        int testRuns = int.Parse(infoXml.Element("LogFileInfo").Element("TestRunCount").Value);
                        for (int i = 1; i <= testRuns; i++)                
                        {
                            ZipEntry zipEntryRun = zipFile.GetEntry($"Run {i}.xml");
                            using (Stream zipStreamRun = zipFile.GetInputStream(zipEntryRun))
                            {
                                using (StreamReader streamReaderRun = new StreamReader(zipStreamRun))
                                {                                    
                                    XDocument runXML = XDocument.Load(streamReaderRun);
                                    UUTReport uut = CreateReport(api, runXML);
                                    api.Submit(uut);
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private UUTReport CreateReport(TDM api, XDocument xml)
        {
            XElement testRun = xml.Element("testrun");
            XElement testRunInfo = testRun.Element("testruninfo");
            UUTReport uut = api.CreateUUTReport(
                testRunInfo.Element("user").Value,
                parameters["partNumber"],
                parameters["partRevision"],
                testRunInfo.Element("serial").Value,
                parameters["operationTypeCode"],
                parameters["sequenceName"],
                parameters["sequenceVersion"]);            
            uut.StartDateTime = DateTime.ParseExact(testRun.Attribute("datetime").Value, "o", CultureInfo.InvariantCulture);
            uut.Comment = testRunInfo.Element("project-description").Value;
            if (testRunInfo.Element("xjlink-name")!=null) uut.StationName = testRunInfo.Element("xjlink-name").Value;
            if (testRunInfo.Element("time-taken") != null)
                uut.ExecutionTime = double.Parse(testRunInfo.Element("time-taken").Value) / 1000.0;
            ReadTests(uut, testRun);
            uut.Status = testRunInfo.Element("result").Value == "Passed"?UUTStatusType.Passed:UUTStatusType.Failed; //Termiated, Error?
            return uut;
        }

        private void ReadTests(UUTReport uut, XElement testRun)
        {
            foreach (XElement testGroup in testRun.Elements("testgroup"))
            {
                SequenceCall sequence = uut.GetRootSequenceCall().AddSequenceCall(testGroup.Attribute("name").Value);
                foreach (XElement testFunction in testGroup.Elements("testfunction"))
                {
                    XElement result = testFunction.Element("result");
                    PassFailStep step = sequence.AddPassFailStep(testFunction.Attribute("name").Value);
                    StepStatusType stepStatus = (StepStatusType)Enum.Parse(typeof(StepStatusType), result.Attribute("value").Value);
                    step.AddTest(stepStatus==StepStatusType.Passed, stepStatus);
                    step.ReportText = testFunction.Element("text-output").Value;
                    if (testFunction.Element("time-taken") != null)
                        step.StepTime = double.Parse(testFunction.Element("time-taken").Value) / 1000.0;
                }
            }
        }
    }
}
