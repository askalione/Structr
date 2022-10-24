# XML-file settings provider

`XmlSettingsProvider` provides functionality for access to a XML-file with settings.

## Setup

Create settings class.

```csharp
public class SmtpEmailSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
}
```

Create XML file with settings: 

```xml
<?xml version="1.0" encoding="utf-16"?>
<SmtpEmailSettings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Host>smtp.example.com</Host>
  <Port>25</Port>
</SmtpEmailSettings>
```

Setup XML-file settings provider:

```csharp
services.AddConfiguration()
    .AddXml<SmtpEmailSettings>("path_to_xml_file");
```