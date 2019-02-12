namespace Ouderbijdrage
{
    class Subscription
    {
        private string name;
        private double price;

        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        public double Price
        {
            set { price = value; }
            get { return price; }
        }

        public Subscription(string subsciptionName, double subscriptionPricce)
        {
            name = subsciptionName;

            price = subscriptionPricce;
        }
    }
}
