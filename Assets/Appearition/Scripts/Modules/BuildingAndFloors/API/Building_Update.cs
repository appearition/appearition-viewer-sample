using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Building/Update/0 , where 0 is Channel ID
    ///
    /// The post data is a Floor object.
    /// </summary>
    [System.Serializable]
    public class Building_Update : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
    }
}