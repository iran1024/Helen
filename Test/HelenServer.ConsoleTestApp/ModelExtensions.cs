using HelenServer.BugEngine.Contracts;
using HelenServer.BugEngine.Dal;

namespace HelenServer.ConsoleTestApp
{
    public static class ModelExtensions
    {
        public static BugModel ToModel(this ZentaoBug bug)
        {
            var model = new BugModel();

            model.Id = bug.Id;
            model.Title = bug.Title;
            model.Product = ConvertOrAddProduct(bug.Product).ToModel();
            model.Module = ConvertOrAddModule(bug.Module);

            return model;
        }

        public static ProductModel ToModel(this Product product)
        {
            return new ProductModel()
            {
                Id = product.Id,
                Name = product.Name
            };
        }

        public static Product ConvertOrAddProduct(string value)
        {
            var target = Program.Context.Product.FirstOrDefault(n => n.Name == value);

            if (target is null)
            {
                var pro = new Product
                {
                    Name = value
                };

                Program.Context.Product.Add(pro);

                return pro;
            }
            else
            {
                return target;
            }
        }

        public static ModuleModel ConvertOrAddModule(string value)
        {
            var target = Program.Context.Module.FirstOrDefault(n => n.Name == value);

            if (target is null)
            {
                var pro = new Product
                {
                    Name = value
                };

                Program.Context.Product.Add(pro);

                return new ModuleModel
                {
                    //Id = 
                };
            }
            else
            {
                return default;
            }
        }
    }
}
