using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class ValidationContext
    {
        private Dictionary<string, string> errors = new Dictionary<string, string>();
        private Queue<string> validationPath = new Queue<string>();
        private string validationPathValue = string.Empty;

        public bool IsValid
        {
            get { return this.errors.Any() == false; }
        }

        public IReadOnlyDictionary<string, string> Errors
        {
            get { return this.errors; }
        }

        public void PushPath(string path)
        {
            this.validationPath.Enqueue(path);
            this.validationPathValue = string.Join("/", this.validationPath);
        }

        public void PopPath()
        {
            this.validationPath.Dequeue();
            this.validationPathValue = string.Join("/", this.validationPath);
        }

        public void Require(string key)
        {
            this.Add(key, "A value is required");
        }

        public void NotSupported(string key, object value)
        {
            this.Add(key, InvariantString.Format("{0} is not a supported value", value ?? "<NULL>"));
        }

        public void Minimum(string key, object min)
        {
            this.Add(key, InvariantString.Format("Must be at least {0}", min));
        }

        public void Add(string key, string error)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key is required", "key");
            }

            if (string.IsNullOrWhiteSpace(error))
            {
                throw new ArgumentException("Error is required", "error");
            }

            this.errors.Add(this.validationPathValue + "/" + key, error);
        }

        public InvalidOperationException CreateException(string message)
        {
            var invalidOpEx = new InvalidOperationException(message);
            
            foreach (var kv in this.errors)
            {
                invalidOpEx.Data.Add(kv.Key, kv.Value);
            }

            return invalidOpEx;
        }
    }
}
