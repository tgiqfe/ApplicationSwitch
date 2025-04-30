namespace ApplicationSwitch.Lib.Rules
{
    public class AppConfig
    {
        /// <summary>
        /// Metadata info
        /// </summary>
        public AppConfigMetadata Metadata { get; set; }

        /// <summary>
        /// Client hostnamr enable / disable target.
        /// </summary>
        public AppConfigTarget Target { get; set; }

        /// <summary>
        /// Rule info
        /// </summary>
        public AppConfigRule Rule { get; set; }
    }
}
