using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/UploadMedia/0 , where 0 is Channel ID and arTargetId is the target ID
    /// </summary>
    [System.Serializable]
    public class BuildingMedia_Add : BaseApiPost
    {
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
        public override int ApiVersion => 2;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int buildingId;
        }

        public override TypeOfPost FormType
        {
            get { return TypeOfPost.MultiForms; }
        }

        //Variables
        public BuildingMedia Data;
    }
}