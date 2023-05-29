using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.BuildingsAndFloors
{
    public static class BuildingConstants
    {
        #region Log Messages

        #region Building

        public const string BUILDING_NULL_BUILDING = "No building data was provided. Aborting process.";

        public const string BUILDING_CREATE_NULL_BUILDING = "No data was provided when trying to create a new building.";
        public const string BUILDING_CREATE_SUCCESS = "A new building was successfully created for the building of id {0} in the channel of id {1}.";
        public const string BUILDING_CREATE_FAILURE = "An issue occured when trying to create a building in the channel of id {0}.";

        public const string BUILDING_GET_SUCCESS = "The building of id {0} in channel {1} was successfully fetched!";
        public const string BUILDING_GET_NOT_FOUND = "The request to find the building of id {0} in the channel of id {1} was successful but no matching building was found.";
        public const string BUILDING_GET_FAILURE = "An issue occured when trying to get the building id {0} in channel of id {1}.";

        public const string BUILDING_LIST_SUCCESS = "The list query was successful and found {0} building(s).\n{1}";
        public const string BUILDING_LIST_FAILURE = "An issue occured when trying to list the buildings on the channel of id {0}.\n{1}";

        public const string BUILDING_UPDATE_SUCCESS = "The building of id {0} (from channel {1}) was updated on the EMS successfully.";
        public const string BUILDING_UPDATE_FAILURE = "An issue occured when trying to update the building of id {0} from channel of id {1}.";

        public const string BUILDING_REMOVE_SUCCESS = "The building of id {0} from the channel of id {1} was removed from the EMS successfully.";
        public const string BUILDING_REMOVE_FAILURE = "An issue occured when trying to remove the building of id {0} from the channel of id {1}.";

        #endregion

        #region Floor

        public const string BUILDINGFLOOR_NULL_FLOOR = "No floor data was provided. Aborting process.";

        public const string BUILDINGFLOOR_CREATE_NULL_FLOOR = "No data was provided when trying to create a new floor.";
        public const string BUILDINGFLOOR_CREATE_SUCCESS = "A new floor was successfully created for the building of id {0}; new floor id:{1}, in the channel of id {2}.";
        public const string BUILDINGFLOOR_CREATE_FAILURE = "An issue occured when trying to create a floor on the building of id {0} and channel of id {1}.";

        public const string BUILDINGFLOOR_GET_SUCCESS = "The floor of id {0} and building id {1} in channel {2} was successfully fetched!";
        public const string BUILDINGFLOOR_GET_NOT_FOUND = "The request to find the floor of id {0} in the building of id {1} and channel of id {2} was successful but no matching floor was found.";
        public const string BUILDINGFLOOR_GET_FAILURE = "An issue occured when trying to get the floor of id {0} and building id {1} in channel {2}.";

        public const string BUILDINGFLOOR_LIST_SUCCESS = "The list query was successful and found {0} floor(s)).\n{1}";
        public const string BUILDINGFLOOR_LIST_FAILURE = "An issue occured when trying to list the floors on the channel of id {0}.\n{1}";

        public const string BUILDINGFLOOR_UPDATE_SUCCESS = "The floor of id {0} from the building of id {1} (from channel {2}) was updated on the EMS successfully.";
        public const string BUILDINGFLOOR_UPDATE_FAILURE = "An issue occured when trying to update the floor of id {0} from building of id {1} from the channel of id {2}.";

        public const string BUILDINGFLOOR_REMOVE_SUCCESS = "The floor of id {0} from the building of id {1} (from channel {2}) was removed from the EMS successfully.";
        public const string BUILDINGFLOOR_REMOVE_FAILURE = "An issue occured when trying to remove the floor of id {0} from building of id {1} from the channel of id {2}.";

        #endregion

        #region Building Materials

        public const string BUILDINGMATERIAL_GET_SUCCESS = "The building material of id {0} in channel {1} was successfully fetched!";
        public const string BUILDINGMATERIAL_GET_NOT_FOUND = "The request to find the building material of id {0} in the channel of id {1} was successful but no matching building material was found.";
        public const string BUILDINGMATERIAL_GET_FAILURE = "An issue occured when trying to get the building material id {0} in channel of id {1}.";

        public const string BUILDINGMATERIAL_LIST_SUCCESS = "The list query was successful and found {0} building material(s).\n{1}";
        public const string BUILDINGMATERIAL_LIST_FAILURE = "An issue occured when trying to list the building materials on the channel of id {0}.\n{1}";

        #endregion

        #region Building Equipment

        public const string BUILDINGEQUIPMENT_GET_SUCCESS = "The building equipment of id {0} in channel {1} was successfully fetched!";
        public const string BUILDINGEQUIPMENT_GET_NOT_FOUND =
            "The request to find the building equipment of id {0} in the channel of id {1} was successful but no matching building equipment was found.";
        public const string BUILDINGEQUIPMENT_GET_FAILURE = "An issue occured when trying to get the building equipment id {0} in channel of id {1}.";

        public const string BUILDINGEQUIPMENT_LIST_SUCCESS = "The list query was successful and found {0} building equipment(s).\n{1}";
        public const string BUILDINGEQUIPMENT_LIST_FAILURE = "An issue occured when trying to list the building equipments on the channel of id {0}.\n{1}";

        #endregion

        #region Building Floor Equipment

        public const string BUILDINGFLOOREQUIPMENT_LIST_SUCCESS = "The list query was successful and found {0} building floor equipment(s).";
        public const string BUILDINGFLOOREQUIPMENT_LIST_FAILURE = "An issue occured when trying to list the building floor equipments on the channel of id {0}.";

        #endregion

        #region Building Media

        public const string BUILDING_MEDIA_NULL = "The given building media is null. Aborting process.";
        public const string BUILDING_MEDIA_FILE_NULL = "The given file to upload is null. Aborting process.";
        public const long FILE_UPLOAD_PLACEHOLDER_SIZE = 3000000;
        public const bool allowParallelDownloads = false;

        public const string BUILDING_MEDIA_LIST_SUCCESS = "The list query was successful and found {0} building media(s) in the building of id {1}.\n{1}";
        public const string BUILDING_MEDIA_LIST_FAILURE = "An issue occured when trying to list the building medias from the building of id {0} on the channel of id {1}.";

        public const string BUILDING_MEDIA_GET_SUCCESS = "Building media of id {0} in the building of id {1} was successfully fetched.\n{1}";
        public const string BUILDING_MEDIA_GET_FAILURE = "An issue occured when trying to fetch the building media of id {0} from the building of id {1} on the channel of id {2}.";
        public const string BUILDING_MEDIA_GET_NOT_FOUND = "The request to find the building media of id {0} from the building of id {1} in the channel of id {2} was successful but no matching building was found.";

        public const string BUILDING_MEDIA_ADD_SUCCESS = "The building media of id {0} located in the building of id {1} (from channel {2}) was updated on the EMS successfully.";
        public const string BUILDING_MEDIA_ADD_FAILURE = "An issue occured when trying to add a media to the building of id {0} from channel of id {1}.";
                                    
        public const string BUILDING_MEDIA_REMOVE_SUCCESS = "The building media of id {0} was successfully removed from the building of id {1} from the channel of id {2}.";
        public const string BUILDING_MEDIA_REMOVE_FAILURE = "An issue occured when trying to remove the building media of id {0} from the building of id {1} from the channel of id {2}.";
        
        #endregion

        #endregion

        #region Query Defaults

        /// <summary>
        /// Default query for Building/List APIs processes.
        /// </summary>
        /// <returns></returns>
        public static PostFilterQuery GetDefaultBuildingListQuery()
        {
            return new PostFilterQuery {
                page = 0,
                recordsPerPage = 999
            };
        }

        /// <summary>
        /// Default query for BuildingFloor/List APIs processes.
        /// </summary>
        /// <returns></returns>
        public static PostFilterQuery GetDefaultBuildingFloorListQuery()
        {
            return new PostFilterQuery {
                page = 0,
                recordsPerPage = 999
            };
        }

        /// <summary>
        /// Default query for BuildingMaterial/List APIs processes.
        /// </summary>
        /// <returns></returns>
        public static PostFilterQuery GetDefaultBuildingMaterialListQuery()
        {
            return new PostFilterQuery {
                page = 0,
                recordsPerPage = 999
            };
        }

        /// <summary>
        /// Default query for BuildingEquipment/List APIs processes.
        /// </summary>
        /// <returns></returns>
        public static PostFilterQuery GetDefaultBuildingEquipmentListQuery()
        {
            return new PostFilterQuery {
                page = 0,
                recordsPerPage = 999
            };
        }

        #endregion
    }
}