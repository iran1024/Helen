namespace HelenServer.BugEngine.Contracts
{
    public class BugResolution
    {
        private static readonly IDictionary<int, BugResolution> _mapper = new Dictionary<int, BugResolution>();
        private readonly string _contract = string.Empty;
        private int? _duplicatedId = null!;

        public int? DuplicatedId => _duplicatedId;

        public static BugResolution AsDesign { get; } = new BugResolution("设计如此");
        public static BugResolution Duplicated { get; } = new BugResolution("重复Bug");
        public static BugResolution External { get; } = new BugResolution("外部原因");
        public static BugResolution Resolved { get; } = new BugResolution("已解决");
        public static BugResolution Irreproducible { get; } = new BugResolution("无法重现");
        public static BugResolution Postponed { get; } = new BugResolution("延期处理");
        public static BugResolution WontFix { get; } = new BugResolution("不予解决");

        static BugResolution()
        {
            _mapper.Add(1, AsDesign);
            _mapper.Add(2, Duplicated);
            _mapper.Add(3, External);
            _mapper.Add(4, Resolved);
            _mapper.Add(5, Irreproducible);
            _mapper.Add(6, Postponed);
            _mapper.Add(7, WontFix);
        }

        private BugResolution() { }

        private BugResolution(string contract)
        {
            _contract = contract;
        }

        public static BugResolution? Parse(string text)
        {
            return _mapper.Values.FirstOrDefault(n => n._contract == text);
        }

        public BugResolution SetDuplicatedId(int duplicatedId)
        {
            if (!_contract.Equals("重复Bug"))
            {
                throw new InvalidOperationException("只有重复Bug可以设置重复Id");
            }

            _duplicatedId = duplicatedId;

            return this;
        }

        public string Contract() => _contract;

        public override string ToString()
        {
            return _mapper.Keys.First(n => _mapper[n]._contract == _contract).ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj is BugResolution resolution &&
                   _contract == resolution._contract &&
                   _duplicatedId == resolution._duplicatedId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_contract, _duplicatedId);
        }
    }
}