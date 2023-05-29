using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.Location
{
    public static class LocationConstants
    {
        #region Log Messages

        public const string LOCATION_NULL = "Null Location provided.";

        public const string ADD_PROPERTY_SUCCESS = "The property of key {0} was successfully added to the Location of id {1}.";
        public const string ADD_PROPERTY_FAILURE = "An error occured when trying to add the property of key {0} to the ArTarget of id {1}\n{2}.";

        public const string GET_PROPERTY_SUCCESS = "{0} properties were successfully fetched from the Location of id {1}.";
        public const string GET_PROPERTY_FAILURE = "An error occured when trying to fetch the properties from the Location of id {0}\n{1}.";

        public const string UPDATE_PROPERTY_SUCCESS = "The property of key {0} was successfully updated to the Location of id {1}.";
        public const string UPDATE_PROPERTY_FAILURE = "An error occured when trying to update the property of key {0} to the Location of id {1}\n{2}.";

        public const string DELETE_PROPERTY_SUCCESS = "The property of key {0} was successfully deleted from the Location of id {1}.";
        public const string DELETE_PROPERTY_FAILURE = "An error occured when trying to delete the property of key {0} from the Location of id {1}\n{2}.";
        
        #endregion
    }
}