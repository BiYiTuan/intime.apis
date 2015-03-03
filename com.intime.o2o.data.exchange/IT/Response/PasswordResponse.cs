namespace com.intime.o2o.data.exchange.IT.Response
{
    public class PasswordResponse : GiftCardResponse
    {
        private string _message;

        public string ErrorMessage
        {
            get {
                if (Initialized)
                {
                    this.Initialize();
                }
                return _message;
            }
        }

        private void Initialize()
        {
            var slices = this.Data.ParseDesc();
            if (slices.Length > 1)
            {
                _message = slices[1];
            }
            this.Initialized = true;
        }
    }
}