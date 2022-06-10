namespace HelenServer.UserCenter.Shared
{
    public enum UserStatus : byte
    {
        /// <summary>
        /// 在岗
        /// </summary>
        OnDuty = 0x1,
        /// <summary>
        /// 请假
        /// </summary>
        Leave,
        /// <summary>
        /// 出差
        /// </summary>
        BusinessTravel,
        /// <summary>
        /// 离职
        /// </summary>
        Quit
    }
}
