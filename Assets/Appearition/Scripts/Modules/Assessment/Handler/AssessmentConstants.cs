namespace Appearition.Assessments
{
    public static class AssessmentConstants
    {
        #region Log Messages

        public const string ASSESSMENT_LIST_SUCCESS = "Assessments of the channel {0} have been successfully fetched!";
        public const string ASSESSMENT_LIST_SUCCESS_OFFLINE = "Assessments of the channel {0} have been successfully loaded offline!";
        public const string ASSESSMENT_LIST_FAILURE = "An error occured when trying to fetch the Assessments from the channel of id {0}";

        public const string ASSESSMENT_GENERATE_EMPTY_QUERY = "Please provide at least one Category Id and one Level Of Proficiency Id to generate an assessment.";
        public const string ASSESSMENT_GENERATE_NULL = "Not all required data was provided during the generation process.";
        public const string ASSESSMENT_GENERATE_NO_QANDA = "No Q&A data was found while trying to generate the assessment of id {0} and name {1}";
        public const string ASSESSMENT_GENERATE_SUCCESS = "Assessment of name {0} and id {1} was successfully generated.";
        public const string ASSESSMENT_GENERATE_FAILURE = "Unable to create any assessment with the proficiency levels of id {0} and categores of id {1}.";
        public const string ASSESSMENT_GENERATE_INVALID_GENERATORS = "It seems some generators were unable to get any templates. Ensure there is at least one definition per generator type.";

        public const string ASSESSMENT_SUBMIT_INVALID = "The provided Assessment's data is invalid or not completed.";
        public const string ASSESSMENT_SUBMIT_SUCCESS = "The Assessment of id {0} and name {1} completed by user {2} was successfully submitted to the channel of id {3}";
        public const string ASSESSMENT_SUBMIT_FAILURE = "An error occured when trying to submit the Assessment of id {0} and name {1} to the channel of id {2}";

        #endregion

        #region Generation

        /// <summary>
        /// The default amount of wrong answer to generate for questions which do not force any quantity.
        /// </summary>
        public const int DEFAULT_AMOUNT_OF_WRONG_ANSWERS = 3;
        public const int DEFAULT_AMOUNT_OF_CORRECT_ANSWERS = 1;
        /// <summary>
        /// Amount of categories generated when asking for "any" during the CategoryTypeGenerator.
        /// </summary>
        public const int GENERATOR_CATEGORY_TYPE_ANY_AMOUNT = 3;

        public const string QUESTION_PROPERTY_CORRECT_MESSAGE_KEY = "CorrectMessage";
        public const string QUESTION_PROPERTY_INCORRECT_MESSAGE_KEY = "IncorrectMessage";

        #endregion
    }
}