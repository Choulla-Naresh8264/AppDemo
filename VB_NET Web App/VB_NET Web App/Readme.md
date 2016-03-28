#SmartButton Web Service VB.NET MVC Implementation

##Introduction

This is a guide to using the SmartButton web services (WS). Before we can use the web services to get data from and submit data to the SmartButton platform, we need to first do some configuration.

##Setup

First of all, the web.config file needs to be configured to include the web service token so that the web services can be used.

```xml
<configuration>
  <appSettings>
    <add key="WSToken" value="YourWebServiceTokenHere"/>
  </appSettings>
</configuration>
```

Then, additional configuration needs to be added so that the services can be bound to an endpoint. Notice, for each web service endpoint that is used, another Soap binding must be added to the **web.config** file.

```xml
<configuration>
  <system.serviceModel>
    <bindings>
      <basicHttpBinding>
        <binding name="MemberSecuritySoap">
          <security mode="Transport" />
        </binding>
        <binding name="MemberSecuritySoap1" />
        <binding name="MemberSoap">
          <security mode="Transport" />
        </binding>
        <binding name="MemberSoap1" />
        <binding name="OfferSoap">
          <security mode="Transport" />
        </binding>
        <binding name="OfferSoap1" />
      </basicHttpBinding>
      <customBinding>
        <binding name="MemberSecuritySoap12">
          <textMessageEncoding messageVersion="Soap12" />
          <httpsTransport />
        </binding>
        <binding name="MemberSoap12">
          <textMessageEncoding messageVersion="Soap12" />
          <httpsTransport />
        </binding>
        <binding name="OfferSoap12">
          <textMessageEncoding messageVersion="Soap12" />
          <httpsTransport />
        </binding>
      </customBinding>
    </bindings>
    <client>
      <endpoint address="https://preview.smartbutton.com/WS/MemberSecurity.asmx"
        binding="basicHttpBinding" bindingConfiguration="MemberSecuritySoap"
        contract="SBMemberSecuritySvc.MemberSecuritySoap" name="MemberSecuritySoap" />
      <endpoint address="https://preview.smartbutton.com/WS/MemberSecurity.asmx"
        binding="customBinding" bindingConfiguration="MemberSecuritySoap12"
        contract="SBMemberSecuritySvc.MemberSecuritySoap" name="MemberSecuritySoap12" />
      <endpoint address="https://preview.smartbutton.com/WS/Member.asmx"
        binding="basicHttpBinding" bindingConfiguration="MemberSoap"
        contract="SBMemberSvc.MemberSoap" name="MemberSoap" />
      <endpoint address="https://preview.smartbutton.com/WS/Member.asmx"
        binding="customBinding" bindingConfiguration="MemberSoap12"
        contract="SBMemberSvc.MemberSoap" name="MemberSoap12" />
      <endpoint address="https://preview.smartbutton.com/WS/Offer.asmx"
        binding="basicHttpBinding" bindingConfiguration="OfferSoap"
        contract="SBOfferSvc.OfferSoap" name="OfferSoap" />
      <endpoint address="https://preview.smartbutton.com/WS/Offer.asmx"
        binding="customBinding" bindingConfiguration="OfferSoap12" contract="SBOfferSvc.OfferSoap"
        name="OfferSoap12" />
    </client>
  </system.serviceModel>
</configuration>
```

You will also need to import the `System.Configuration` namespace in each file you plan to use the web services in, so that the code can reference the web service key using the `System.Configuration.ConfigurationManager` class. Example:

```VB.NET
Imports System.Configuration

...

ConfigurationManager.AppSettings("WSToken")
```

##Basic Usage

When using the web services in a VB.NET project, use the following format to utilize the services properly:

```VB.NET
Dim soapClient = New ServiceReferenceName.ServiceSoapClient("SoapBindingName")

Dim serviceMethodReturnObject As ServiceReferenceName.ServiceMethodReturn

serviceMethodReturnObject = soapClient.ServiceMethod(ConfigurationManager.AppSettings("WSToken"), ServiceMethodArg1, ServiceMethodArg2, ServiceMethodArg3, ...)
```

The resulting return object (in this instance the **ValidatePasswordReturn** class) will contain the **ReturnCode** property which will indicate the success of the API call. For a list of return codes used with the web services, reference [this](https://wiki.smartbutton.com/default.asp?W472 "Web Service Return Codes") link.