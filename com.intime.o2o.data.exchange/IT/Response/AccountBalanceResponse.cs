using com.intime.o2o.data.exchange.IT.Response.Entity;

namespace com.intime.o2o.data.exchange.IT.Response
{
    public class AccountBalanceResponse : Response<GeneralResult>
    {
        private decimal _balance;
        public decimal Balance
        {
            get
            {
                if (_balance == 0)
                {
                    this.ParseDesc();
                }
                return _balance;
            }
        }

        private string _message;

        private void ParseDesc()
        {
            var slices = this.Data.ParseDesc();
            if (slices.Length > 2)
            {
                _balance = decimal.Parse(slices[1])/100;
                SetMessage(slices[0], slices[2]);
            }
            else if (slices.Length > 1)
            {
                SetMessage(slices[0], slices[1]);
            }
        }

        private void SetMessage(string statusCode, string message)
        {
            switch (statusCode)
            {
                case "00":
                    this._message = message;
                    break;
                case "01":
                    this._message = "此卡已挂失";
                    break;
                case "02":
                    this._message = "此卡已冻结";
                    break;
                case "03":
                    this._message = "此卡已注销";
                    break;
                case "04":
                    this._message = "已换卡";
                    break;
                case "05":
                    this._message = "此卡未开户";
                    break;
                case "FF":
                    this._message = message;
                    break;

            }
        }

        public string ErrorMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_message))
                {
                    this.ParseDesc();
                }
                return _message;
            }
        }
    }
}
