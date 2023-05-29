namespace Appearition.API
{
    /// <summary>
    /// Container of additional parameters to get added to the request's URL
    /// </summary>
    [System.Serializable]
    public abstract class BaseRequestContent
    {
        /// <summary>
        /// Fetches all variables inside this current class and creates a URL extra parameter string to add to the URL request.
        /// </summary>
        /// <returns>The URL extra parameters.</returns>
        public virtual string GetUrlExtraParameters()
        {
            //Create a blank value
            string output = "";

            //Fetch all the variables present inside the class. For each of them, get the value and name, and create an extra param string
            System.Reflection.FieldInfo[] allVarInfos = GetType().GetFields();

            if (allVarInfos.Length > 0)
            {
                output += "?";

                for (int i = 0; i < allVarInfos.Length; i++)
                {
                    output += allVarInfos[i].Name + "=" + allVarInfos[i].GetValue(this);

                    if (i + 1 < allVarInfos.Length)
                        output += "&";
                }
            }

            //Output result
            return output;
        }
    }
}