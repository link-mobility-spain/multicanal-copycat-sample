using System;
using System.Collections.Generic;
using System.Text;

namespace Link.Multicanal.API
{
    ///

    /// Enum representing message inspection types.
    /// 
    public enum eMessageInspectionType { Request = 0, Response = 1 };

    ///

    /// Class to pass inspection event arguments.
    /// 
    public class MessageInspectorArgs : EventArgs
    {
        ///

        /// Type of the message inpected.
        /// 
        public eMessageInspectionType MessageInspectionType { get; internal set; }

        ///

        /// Inspected raw message.
        /// 
        public string Message { get; internal set; }
    }
}
