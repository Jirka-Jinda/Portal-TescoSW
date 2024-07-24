public class AppSettings
{
    public string schema { get; set; }
    public Applicationserver ApplicationServer { get; set; }
    public Connectionstring[] ConnectionStrings { get; set; }
    public Datasourcesettings DataSourceSettings { get; set; }
    public Logsettings LogSettings { get; set; }
    public Testssettings TestsSettings { get; set; }
    public Cryptographysettings CryptographySettings { get; set; }
    public Dmssettings DMSSettings { get; set; }
    public Datainterchange DataInterchange { get; set; }
    public Owmanagersettings OWManagerSettings { get; set; }
    public Passwordmanagersettings PasswordManagerSettings { get; set; }
}

public class Applicationserver
{
    public string[] AllowedInterfaces { get; set; }
}

public class Datasourcesettings
{
    public Mssqlsetting[] MSSQLSettings { get; set; }
}

public class Mssqlsetting
{
    public string Name { get; set; }
    public string Owner { get; set; }
    public bool UseSequences { get; set; }
}

public class Logsettings
{
    public string AppName { get; set; }
    public string AppLogName { get; set; }
    public string ProcessUId { get; set; }
    public bool LoggingUserErrors { get; set; }
    public bool LoggingDebugMessages { get; set; }
    public bool WriteErrorsToSystemApplicationLog { get; set; }
    public int LogFullCriticalErrorMessageInterval { get; set; }
    public Loggingcommunication LoggingCommunication { get; set; }
    public Textfilelogs TextFileLogs { get; set; }
    public Exceptions Exceptions { get; set; }
    public Criticalerrorcontact[] CriticalErrorContacts { get; set; }
    public Loghttpheader[] LogHttpHeaders { get; set; }
    public Logsource[] LogSources { get; set; }
}

public class Loggingcommunication
{
    public bool IISLogging { get; set; }
    public int TimeForLongRequestWarning { get; set; }
}

public class Textfilelogs
{
    public string Directory { get; set; }
}

public class Exceptions
{
    public string DetailsLevel { get; set; }
}

public class Criticalerrorcontact
{
    public string Email { get; set; }
}

public class Loghttpheader
{
    public string Value { get; set; }
    public bool UseHash { get; set; }
}

public class Logsource
{
    public string Type { get; set; }
    public string Suffix { get; set; }
}

public class Testssettings
{
    public int ResultLifeTime { get; set; }
    public string LogTestResults { get; set; }
    public Test[] Tests { get; set; }
}

public class Test
{
    public string Name { get; set; }
    public int ResultLifeTime { get; set; }
}

public class Cryptographysettings
{
    public int ReTimeStampTime { get; set; }
    public Stringdatasignature StringDataSignature { get; set; }
    public Objectdatasignature ObjectDataSignature { get; set; }
    public Documentdatasignature DocumentDataSignature { get; set; }
    public Encryption Encryption { get; set; }
    public Signingworker[] SigningWorker { get; set; }
    public Certificate[] Certificates { get; set; }
}

public class Stringdatasignature
{
    public string DefaultProfileID { get; set; }
}

public class Objectdatasignature
{
    public string DefaultProfileID { get; set; }
}

public class Documentdatasignature
{
    public string DefaultProfileID { get; set; }
}

public class Encryption
{
    public string Algorithm { get; set; }
}

public class Signingworker
{
    public string Name { get; set; }
    public bool Register { get; set; }
}

public class Certificate
{
    public string ID { get; set; }
    public string FilePath { get; set; }
    public string Password { get; set; }
    public string Type { get; set; }
    public string StoreName { get; set; }
    public string Thumbprint { get; set; }
}

public class Dmssettings
{
    public string HMACKey { get; set; }
    public string ServerURL { get; set; }
}

public class Datainterchange
{
    public string ClientBindingName { get; set; }
    public string ServerEndpoint { get; set; }
}

public class Owmanagersettings
{
    public bool AllowLdap { get; set; }
    public bool AllowApplicationUser { get; set; }
    public bool AllowAnonymousUser { get; set; }
    public bool ConnectionPooling { get; set; }
    public string ChangeLogonDataMethod { get; set; }
    public int RightsVersion { get; set; }
    public bool IgnoreLongTransactionAttribute { get; set; }
    public int LongTransactionTimeout { get; set; }
    public int MaxReportRows { get; set; }
    public int MaxReaderRows { get; set; }
    public int ConditionParserIterationLimit { get; set; }
    public int LoginDelay { get; set; }
    public bool OTPRequired { get; set; }
    public bool TransactionQueue { get; set; }
    public string ExternalProfiling { get; set; }
    public int CacheRefreshInterval { get; set; }
    public string FullRole { get; set; }
    public bool UseDBDateTime { get; set; }
    public bool AuditEnabled { get; set; }
    public string SupportLMT { get; set; }
    public Helpmanager HelpManager { get; set; }
    public string BinaryDataDefaultDMSProviderName { get; set; }
    public Globalvalues GlobalValues { get; set; }
    public Connectionsettings ConnectionSettings { get; set; }
}

public class Helpmanager
{
    public string Class { get; set; }
    public string Method { get; set; }
}

public class Globalvalues
{
    public Applicationid ApplicationID { get; set; }
    public Autosaveprofile AutoSaveProfile { get; set; }
}

public class Applicationid
{
    public string Value { get; set; }
    public string Type { get; set; }
}

public class Autosaveprofile
{
    public string Value { get; set; }
    public string Type { get; set; }
}

public class Connectionsettings
{
    public string BeforeAuthenticationMethod { get; set; }
    public Connection[] Connections { get; set; }
}

public class Connection
{
    public string Type { get; set; }
    public string ConnectionStringName { get; set; }
    public string DataSourceSettingsName { get; set; }
    public string Username { get; set; }
}

public class Passwordmanagersettings
{
    public string PasswordComplexityDefinition { get; set; }
}

public class Connectionstring
{
    public string Name { get; set; }
    public string ConnectionString { get; set; }
    public string ProviderName { get; set; }
}
