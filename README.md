# WATS Client Converter - JTAG

A WATS Client converter plugin for importing import files produced by JTAG test tools to WATS.

## Getting Started

* [About WATS](https://wats.com/manufacturing-intelligence/)
* [About submitting data to WATS](https://virinco.zendesk.com/hc/en-us/articles/207424613)
* [About creating custom converters](https://virinco.zendesk.com/hc/en-us/articles/207424593)

## Download

You can download the latest released version of the converter [here](https://github.com/Virinco/WATS-Client-Converter-JTAG/releases/latest). See the Custom Converter section in the [WATS Client Installation Guide](https://wats.com/download) for your version of the WATS Client for how to install a converter.

### Parameters

This converter uses the following parameters:

| Parameter         | Default value | Description                                                    |
|-------------------|---------------|----------------------------------------------------------------|
| partNumber        | PN            | If log is missing a part number, use this one.                 |
| partRevision      | 1.0           | If log is missing a revision, use this one.                    |
| operationTypeCode | 10            | If log is missing operation code (process code), use this one. |
| sequenceName      | JTAGSeq1      | If log is missing sequence name, use this one.                 |
| sequenceVersion   | 1.0.0         | If log is missing sequence version, use this one.              |

## Testing

The project uses the [MSTest framework](https://docs.microsoft.com/en-us/visualstudio/test/quick-start-test-driven-development-with-test-explorer) for testing the converter.

It is setup with two tests; one for setting up the API by registering the client to your WATS, and one for running the converter.

The values are hardcoded in the test, so you will need to change the values to reflect your setup.
* In SetupClient, fill in your information in the the call to RegisterClient.
* In TestConverter, fill in the path to the file you want to test the converter with. There are example files in the Examples folder.
* Run SetupClient once, then you can run TestConverter as many times as you want.

## Contributing

We're open to suggestions! Feel free open an issue or create a pull request.

Please read [Contributing](CONTRIBUTING.md) for details on contributions.

## Troubleshooting

#### Converter failed to start

Symptom:
* Converter did not start after being configured.

Possible cause:
* WATS Client Service does not have folder permission to the input path.
* WATS Client Service was not restarted after configuration.

Possible solution:
* [Give NETWORK SERVICE write permission to the input path folder](https://virinco.zendesk.com/hc/en-us/articles/207424113-WATS-Client-Add-write-permission-to-NETWORK-SERVICE-on-file-system-to-allow-converter-access)
* Make a change in a converter configuration and undo the change, click APPLY. When asked to restart the service, click Yes.

#### Converter class drop down list is empty

Symptom:
* The converter class drop down list in the Client configurator is empty after adding a converter DLL.

Possible cause:
* The DLL file is blocked. Windows blocks files that it thinks are untrusted, which stops them from being executed.

Possible solution:
* Open properties on the file and unblock it.

#### Other

Contact Virinco support, and include the wats.log file: [Where to find the wats log file at the Client](https://virinco.zendesk.com/hc/en-us/articles/207424033-Where-to-find-the-wats-log-file-at-the-Client).

## Contact

* Issues with the converter or suggestions for improvements can be submitted as an issue here on GitHub.
* Ask questions about WATS in the [WATS Community Help](https://virinco.zendesk.com/hc/en-us/community/topics/200229613)
* Suggestions for the WATS Client itself or WATS in general can be submitted to the [WATS Idea Exchange](https://virinco.zendesk.com/hc/en-us/community/topics/200229623)
* Sensitive installation issues or other sensitive questions can be sent to [support@virinco.com](mailto://support@virinco.com)

## License

This project is licensed under the [LGPLv3](COPYING.LESSER) which is an extention of the [GPLv3](COPYING).