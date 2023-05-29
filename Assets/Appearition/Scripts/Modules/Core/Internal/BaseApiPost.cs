namespace Appearition.API
{
    /// <summary>
    /// Parent class of HTTP POST requests.
    /// </summary>
    [System.Serializable]
    public abstract class BaseApiPost : BaseApi
    {
        //Type of POST
        public virtual TypeOfPost FormType
        {
            get { return TypeOfPost.SingleForm; }
        }
    }
}