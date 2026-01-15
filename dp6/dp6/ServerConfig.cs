namespace dp6;

/// <summary>
/// Immutable konfigurace serveru
/// </summary>
public sealed class ServerConfig
{
    public string Address { get; }
    public int Port { get; }
    public bool IsEncryptionEnabled { get; }
    public int MaxConnections { get; }
    public int TimeoutSeconds { get; }
    public bool IsLoggingEnabled { get; }

    private ServerConfig(string address, int port, bool isEncryptionEnabled, 
        int maxConnections, int timeoutSeconds, bool isLoggingEnabled)
    {
        Address = address;
        Port = port;
        IsEncryptionEnabled = isEncryptionEnabled;
        MaxConnections = maxConnections;
        TimeoutSeconds = timeoutSeconds;
        IsLoggingEnabled = isLoggingEnabled;
    }

    public static ServerConfigBuilder CreateBuilder() => new ServerConfigBuilder();

    public override string ToString()
    {
        return $"Server at {Address}:{Port} " +
               $"[Encryption: {(IsEncryptionEnabled ? "ON" : "OFF")}, " +
               $"MaxConnections: {MaxConnections}, " +
               $"Timeout: {TimeoutSeconds}s, " +
               $"Logging: {(IsLoggingEnabled ? "ON" : "OFF")}]";
    }

    public sealed class ServerConfigBuilder
    {
        private string? _address;
        private int? _port;
        private bool _isEncryptionEnabled = false;
        private int _maxConnections = 100;
        private int _timeoutSeconds = 30;
        private bool _isLoggingEnabled = false;

        internal ServerConfigBuilder() { }

        public ServerConfigBuilder WithAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Adresa serveru nesmí být prázdná", nameof(address));
            
            _address = address;
            return this;
        }

        public ServerConfigBuilder WithPort(int port)
        {
            if (port <= 0 || port > 65535)
                throw new ArgumentException("Port musí být v rozsahu 1-65535", nameof(port));
            
            _port = port;
            return this;
        }

        public ServerConfigBuilder WithEncryption(bool enabled = true)
        {
            _isEncryptionEnabled = enabled;
            return this;
        }

        public ServerConfigBuilder WithMaxConnections(int maxConnections)
        {
            if (maxConnections <= 0)
                throw new ArgumentException("Maximální poèet spojení musí být kladný", nameof(maxConnections));
            
            _maxConnections = maxConnections;
            return this;
        }

        public ServerConfigBuilder WithTimeout(int timeoutSeconds)
        {
            if (timeoutSeconds <= 0)
                throw new ArgumentException("Timeout musí být kladný", nameof(timeoutSeconds));
            
            _timeoutSeconds = timeoutSeconds;
            return this;
        }

        public ServerConfigBuilder WithLogging(bool enabled = true)
        {
            _isLoggingEnabled = enabled;
            return this;
        }

        public ServerConfig Build()
        {
            if (string.IsNullOrWhiteSpace(_address))
                throw new InvalidOperationException("Adresa serveru je povinná");
            
            if (!_port.HasValue)
                throw new InvalidOperationException("Port je povinný");

            return new ServerConfig(
                _address,
                _port.Value,
                _isEncryptionEnabled,
                _maxConnections,
                _timeoutSeconds,
                _isLoggingEnabled
            );
        }
    }
}
