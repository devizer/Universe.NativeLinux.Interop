using System;

namespace Universe.LinuxTaskStats
{
    /// <summary>
    /// By default this exception is never thrown. ErrorNumber property provides details:  
    /// <list type="bullet">
    /// <listheader>
    /// <term>0</term>
    /// <description>OK, taskstat structure successfully retrieved</description>
    /// </listheader>
    /// <item>
    /// <term>1</term>
    /// <description>Either pid or tid arguments expected</description>
    /// </item>
    /// <item>
    /// <term>2</term>
    /// <description>Error creating Netlink Socket 'create_nl_socket'</description>
    /// </item>
    /// <item>
    /// <term>3</term>
    /// <description>Error getting family id 'get_family_id(nl_sd)'</description>
    /// </item>
    /// <item>
    /// <term>4</term>
    /// <description>Error sending tid/tgid cmd 'send_cmd(...)'</description>
    /// </item>
    /// <item>
    /// <term>8</term>
    /// <description>Fatal Reply Error. NLMSG_ERROR Recieved.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class TaskStatInteropException : Exception
    {
        public int ErrorNumber { get; set; }
        
        public TaskStatInteropException()
        {
        }

#if !NETSTANDARD1_1 && !NETCOREAPP1_0 && !NETCOREAPP1_1
        protected TaskStatInteropException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
#endif

        public TaskStatInteropException(string message) : base(message)
        {
        }

        public TaskStatInteropException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TaskStatInteropException(int errorNumber) : this(GetErrorMessage(errorNumber))
        {
            ErrorNumber = errorNumber;
        }

        static string GetErrorMessage(int errorNumber)
        {
            string desc = GetErrorDescription(errorNumber);
            return "Unable to obtain taskstat structure"
                   + (desc == null ? $". Error Code {errorNumber}" : $". Error Code {errorNumber}. {desc}.");
        }
        
        static string GetErrorDescription(int errorNumber)
        {
            switch (errorNumber)
            {
                case 8: return "Fatal Reply Error. NLMSG_ERROR Recieved.";
                case 4: return "Error sending tid/tgid cmd 'send_cmd(...)'";
                case 3: return "Error getting family id 'get_family_id(nl_sd)'";
                case 2: return "Error creating Netlink Socket (create_nl_socket)";
                default: return null;
            }
        }


    }
}