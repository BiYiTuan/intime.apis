
namespace com.intime.o2o.data.exchange.IT.Response
{
    public class RechargeResponse : GiftCardResponse
    {
        private string _message;
        private int _balance;
        private string _account;
        private int _rechargeAmount;

       public string ErrorMessage {
            get
            {
                if (!Initialized)
                {
                    this.Initialize();
                }
                return _message;
            }
        }

       public int Balance {
           get
           {
               if (!Initialized)
               {
                   this.Initialize();

               }
               return _balance;
           }
       }

        public int RechargeAmount
        {
            get
            {
                if (!Initialized)
                {
                    this.Initialize();
                }
                return _rechargeAmount;
            }
        }

        public string Account
        {
            get
            {
                if (!Initialized)
                {
                    this.Initialize();
                }
                return _account;
            }
        }

        private void Initialize()
        {
            var slices = this.Data.ParseDesc();
            if (slices.Length > 4)
            {
                this._account = slices[1];
                this._balance = int.Parse(slices[3]);
                this._rechargeAmount = int.Parse(slices[2]);
                this._message = slices[4];
            }
            else if (slices.Length > 1)
            {
                this._message = slices[1];
            }
            this.Initialized = true;
        }
    }
}
