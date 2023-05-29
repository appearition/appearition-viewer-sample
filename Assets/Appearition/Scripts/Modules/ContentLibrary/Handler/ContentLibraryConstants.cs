using Appearition.ContentLibrary.API;

namespace Appearition.ContentLibrary
{
    public static class ContentLibraryConstants
    {
        public const string NO_PROVIDER_NAME_PROVIDED = "No provider name has been provided as part of this process.";
        public const string CONTENT_FILE_DOWNLOAD_NO_FILE_GIVEN = "The file to download is null.";
        
        public const string GET_CONTENT_ITEM_SUCCESS_OFFLINE = "Content item of the channel {0} have been successfully fetched offline!";
        public const string GET_CONTENT_ITEM_SUCCESS = "Content item of the channel {0} have been successfully fetched!";
        public const string GET_CONTENT_ITEM_FAILURE = "An error occured when trying to fetch the content item from the channel of id {0}";

        public const string GET_PROVIDERS_SUCCESS = "Content Providers have been successfully fetched!";
        public const string GET_PROVIDERS_FAILURE = "An error occured when trying to fetch the content providers from the channel of id {0}";

        public const string SEARCH_SUCCESS = "Content library item search of provider of name {0} has concluded with {1} items!\nQuery: {2}";
        public const string SEARCH_SUCCESS_EMPTY = "The search of content library items for the provider of name {0} has concluded with no items found.\nQuery: {1}";
        public const string SEARCH_FAILURE = "An error occured when during the search of content library items for the provider of name {0} on channel {1}.\nQuery: {2}";

        /// <summary>
        /// Default query for ArTarget/List APIs processes.
        /// </summary>
        /// <returns></returns>
        public static Content_Search.QueryContent GetDefaultSearchQuery()
        {
            return new Content_Search.QueryContent() {
                Page = 0,
                RecordsPerPage = 999,
            };
        }
    }
}