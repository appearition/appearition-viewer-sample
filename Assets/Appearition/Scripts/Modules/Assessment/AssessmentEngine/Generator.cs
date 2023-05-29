using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Globalization;
using Appearition.Assessments;
using Appearition.Learn;
using Appearition.QAndA;
using UnityEngine;

namespace Appearition.Assessments
{
	public class Generator
	{
		#region Parse Text Helpers

		private static class TextVariables
		{
			public const string Joiner = ", ";

			public static class OfNode
			{
				public const string AnswerPart = "answer_part";
				public const string HighlightedPart = "highlighted_part";
				public const string SelectedPart = "selected_part";
				public const string SelectedAnswer = "selected_answer";

			}

			public static class OfAssessment
			{
				public const string AssessmentCategory = "Assessment_Category";
				public const string Category = "Category";
				public const string AssessmentName = "Assessment_Name";
				public const string TargetProficiencyLevel = "Proficiency_Level";
				public const string FinalScore = "Score";
				public const string FinalScoreValue = "Score_Value";
				public const string FinalScorePercent = "Score_Percent";
			}
		}

		/// <summary>
		/// Replace any spaces with an underscore e.g. node.characteristic 1 => node.characteristic_1
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private string CleanNodePropertyKey (string key)
		{
			return Regex.Replace (key, @"\s+", "_").ToLowerInvariant ();
		}

		private string GetNodeValue (LearnNode node, string propertyKey)
		{
			var retVal = node.name;

			// So if we have a node property, go off and get it
			if (string.IsNullOrEmpty (propertyKey))
				return retVal;

			var nodeProperty =
				node.nodeProperties.FirstOrDefault (x =>
                    CleanNodePropertyKey (x.propertyKey).Equals (propertyKey, StringComparison.OrdinalIgnoreCase));

			if (nodeProperty != null
			    && nodeProperty.propertyValue != null
			    && nodeProperty.propertyValue.Length > 0) {
				retVal = nodeProperty.propertyValue;
			}

			return retVal;
		}

		#endregion

		/// <summary>
		/// Call this method to replace any "variables" within the text with actual values from the given parameter sets.
		/// A "variable" will take the format of "[[some_variable_name]]"". This method knows what all these variables are
		/// and how to replace them.
		/// </summary>
		/// <param name="text">The message which has variables that needs to be translated. If there are no variables, then simply the same string will be echoed back.</param>
		/// <param name="relatedNodes">A collection of nodes which have been presented to the user as part of the questions.</param>
		/// <param name="questionOptions">A collection of options which have been presented to the user as part of the questions where one or some are correct.</param>
		/// <param name="assessmentCategory">Contains the category and proficiency level of the current question being asked.</param>
		/// <param name="answerAttempt">This holds the answer which has been given by the student. It may have the node that we selected and/or the text answer chosen</param>
		/// <param name="studentAssessment">This is the overall view model that the generator has created and returned and should be used for updating as the student
		/// progresses through the questions. At the end of all the quiz, this will be populated with the final score and message. Note, the message shown at the end of
		/// quiz should be parsed through this method as some of the text may have variables so that the final score and other values can be set from the assessment.
		/// Hence in that case, make sure you have updated as much as you can, including final score, before invoking this method to parse the message shown to the student.
		/// </param>
		/// <returns></returns>
		public string ParseText (
			string text,
			IList<AssessmentSubmissionQuestionRelatedNode> relatedNodes,
			IList<AssessmentSubmissionQuestionOption> questionOptions,
			AssessmentSubmissionCategory assessmentCategory,
			AssessmentSubmissionQuestionAttempt answerAttempt,
			AssessmentSubmissionData studentAssessment = null)
		{
			// So, in the text the variable will appear in 2 formats:
			// a) [[variable_name]]                  i.e. wrapped in double square brackets
			// b) [[variable_name.property_name]]    i.e. we need to replace this with the value of NodeProperty

			if (string.IsNullOrEmpty(text))
				return "";

			var replaceDictionary = new NameValueCollection ();

			// First, get all variables in the text
			Regex rx = new Regex (@"\[\[(.*?)\]\]");

			foreach (Match match in rx.Matches(text)) {
				// we want to replace all the variables at the end.
				if (!replaceDictionary.AllKeys.Any (x => x.Equals (match.Value, StringComparison.OrdinalIgnoreCase)))
					replaceDictionary.Add (match.Value, match.Value);

				// Strip away the brackets to just leave the variable name
				var variableClean = match.Value.Replace ("[[", string.Empty).Replace ("]]", string.Empty);

				// Look for a property (i.e. delimited by a dot)
				var variableArr = variableClean.Split ('.');
				var variable = variableArr [0];
				var nodePropertyKey = string.Empty;
				if (variableArr.Length == 2)
					nodePropertyKey = CleanNodePropertyKey (variableArr [1]);

				var valueToReplace = string.Empty;
				
				switch (variable) {
				case TextVariables.OfNode.HighlightedPart: // i.e. it's a related part
					if (relatedNodes == null)
						break;

			            // there is usually just one highlighted part, but it is possible
			            // from a data model and schema perspective for there to be multiple.
			            // In such a case, we will concatenate all the highlighted parts together
					foreach (var relatedNode in relatedNodes.Where(x => x.isHighlighted)) {
						if (valueToReplace.Length > 0)
							valueToReplace += TextVariables.Joiner;

						// there must be a node... yes we could use the relatedNode.NodeName however
						// something is wrong if it is not found in the node collection
						var nodeToReplace = _nodes.FirstOrDefault (x => x.nodeId == relatedNode.nodeId);
						if (nodeToReplace == null)
							continue;

						// default it to the node name
						valueToReplace += GetNodeValue (nodeToReplace, nodePropertyKey);
					}

					break;
				case TextVariables.OfNode.AnswerPart:
					if (questionOptions == null)
						break;

			            // there is usually just one highlighted part, but it is possible
			            // from a data model and schema perspective for there to be multiple.
			            // In such a case, we will concatenate all the highlighted parts together
					foreach (var questionOption in questionOptions.Where(x => x.isCorrect)) {
						if (valueToReplace.Length > 0)
							valueToReplace += TextVariables.Joiner;

						// there might be a node or there may simple be text
						var iHaveOptionText = false;
						if (!string.IsNullOrEmpty(questionOption.optionText)) {
							valueToReplace += questionOption.optionText;
							iHaveOptionText = true;
						}

						if (questionOption.nodeId == 0)
							continue;

						var nodeOption = _nodes.FirstOrDefault (x => x.nodeId == questionOption.nodeId);
						if (nodeOption != null) {
							if (iHaveOptionText)
								valueToReplace += TextVariables.Joiner;

							valueToReplace += GetNodeValue (nodeOption, nodePropertyKey);
						}
					}

					break;

				case TextVariables.OfNode.SelectedPart:
				case TextVariables.OfNode.SelectedAnswer:

					if (answerAttempt == null)
						break;
					valueToReplace = answerAttempt.answerText;
					if (answerAttempt.nodeId == 0)
						break;
					var node = _nodes.FirstOrDefault (x => x.nodeId == answerAttempt.nodeId);
					if (node == null)
						break;
					if (!string.IsNullOrEmpty (valueToReplace))
						valueToReplace += TextVariables.Joiner;
					valueToReplace += GetNodeValue (node, nodePropertyKey);
					break;

				case TextVariables.OfAssessment.AssessmentCategory:
				case TextVariables.OfAssessment.Category:
					var category =
						Categories.FirstOrDefault (x => x.nodeCategoryId == assessmentCategory.categoryId);
					if (category == null)
						break;
					valueToReplace = category.name;
					break;

				case TextVariables.OfAssessment.AssessmentName:
					if (studentAssessment == null)
						break;
					valueToReplace = studentAssessment.assessmentName;
					break;

				case TextVariables.OfAssessment.TargetProficiencyLevel:
					if (studentAssessment == null)
						break;
					var pl = ProficiencyLevels.FirstOrDefault (x =>
			                x.Id == studentAssessment.targetProficiencyLevelId);
					if (pl == null)
						valueToReplace = studentAssessment.targetProficiencyLevelId.ToString ();
					else
						valueToReplace = string.IsNullOrEmpty (pl.Name)
			                    ? studentAssessment.targetProficiencyLevelId.ToString ()
			                    : pl.Name;

					break;

				case TextVariables.OfAssessment.FinalScoreValue:
					if (studentAssessment == null)
						break;
					valueToReplace = studentAssessment.achievedScoreValue.ToString (CultureInfo.InvariantCulture);
					break;

				case TextVariables.OfAssessment.FinalScore:
				case TextVariables.OfAssessment.FinalScorePercent:
					if (studentAssessment == null)
						break;
					valueToReplace = studentAssessment.achievedScorePercentage.ToString (CultureInfo.InvariantCulture);
					break;
				}

				replaceDictionary [match.Value] = !string.IsNullOrEmpty (valueToReplace) ? valueToReplace : match.Value;
			}

			// Do the replacement
			foreach (string key in replaceDictionary.Keys) {
				text = text.Replace (key, replaceDictionary [key]);
			}

			//Debug.Log ("Parse outcome: " + text);
			return text;
		}


		/// <summary>
		/// Builds the set of questions to present and collect answers for the given assessment and user.
		///
		/// </summary>
		/// <param name="username"></param>
		/// <param name="assessmentId"><see cref="ListAssessmentOptions"/></param>
		///  <returns>
		/// The ViewModel which will contain all of the questions in correct order and the view model which
		/// needs to be sent up to the server at the end when we submit the assessment.
		/// The ViewModel will have fields already populated from the assessment (i.e. assessment name, target proficiency level,
		/// and max achievable score). It will also have the student username set (the one passed in as the parameter) .
		/// The other fields need to be populated by you (the conusumer of this method): Start Date and Time, End Date and time, the overall
		/// achieved score (which you will calculate as the questions are answered and scores awarded for each correct answer) and
		/// finally, at the end of the assessment you will need to populate the Achieved Score Percentage). There is one final field: MessageShow.
		/// This field will hold the actual message you will show the student once they have completed the assessment. The choice of message to show
		/// is actually also contained for you within this view model (see the AssessmentScores). Assessment scores are important because depending
		/// on the final percentage achieved by the student, a different message will be shown. The "AssessmentScores" is a collected of score ranges
		/// with a message text. Once at the end, after you have calculated the final percentage (i.e. (Max Achievable Score / Achieved Score) * (100/1) ),
		/// you will look up the "AssessmentScores" collection and pick up the MessageToShow field.  But wait there is one more thing before you show that
		/// message to the studuent. Message texts may contain special variables in the format of "[[some variable]]". This is form of templated message whereby
		/// the variables must be replaced with actual values before we show it to the student. So, what are these variables and how do you replace them?
		/// Well the answer is easy really... you don't have to worry about it... the magic is taken care of by the method <see cref="ParseText"/>. Simply call
		/// this method, pass in the message and the variables it needs and it will return you a formatted string.
		///
		/// The header will contain a collection of categories (i.e. StudentAssessmentCategories). Questions are grouped according to Category and Proficiency level.
		/// As such, we want to collect and record scores at this level. Therefore, for each "StudentAssessmentCategory" you will find "Max Achievable Score" and
		/// "Achieved Score". The max score will be prefilled for you, but you will need to populate the "Achieved Score" value. This will simply be a sum of scores
		/// from each question under this "StudentAssessmentCategory". This now brings me to the set of questions. There is a collection named "StudentAssessmentQuestions"
		/// and this holds all of the questions, it correct order, with all of the information you will need to render options, show the relevant parts of the 3D model.
		/// It also has instructions on how to score the student for each attempt they make. Finally, each question has also has a specific message to show if you get
		/// it right or wrong. The message lives in the "QuestionProperties" collection, where the ProperyKey you need to look for is "CorrectMessage" or "IncorrectMessage".
		/// The text for these messages are in the ProperyValue field and once again, you will need to parse this text through the <see cref="ParseText"/> method to replace
		/// any variables with appropriate values
		///
		/// <exception cref="GeneratorException"></exception>
		/// </returns>
		public AssessmentSubmissionData GenerateAssessment (string username, long assessmentId)
		{
			// Firstly we must have the assessment being asked for
			var assessment = _assessments.FirstOrDefault (x => x.assessmentId == assessmentId);
			if (assessment == null)
				throw new GeneratorException ("Assessment Id not found in list of assessments.");

			// This ViewModel will be returned at the end. It will be built up as we go done the logic
			// Firstly we prime the header information
			var studentAssessment = new AssessmentSubmissionData(assessment) {
				productId = assessment.productId,
				studentUsername = username,
				assessmentId = assessment.assessmentId,
				assessmentName = assessment.assessmentName,
				targetProficiencyLevelId = assessment.targetProficiencyLevelId,
				targetProficiencyLevelName = assessment.targetProficiencyLevel,
                studentAssessmentCategories = new List<AssessmentSubmissionCategory>(),
			};

			// As we go along below and create questions under each Category and Proficiency level, we
			// want to preserve the order of questions for the overall assessment across all categories and proficiency levels
			var questionOrdinalPosition = 0;

			// For the given assessment, there will be intructions on which categories to pick questions from and how
			// to go about randomising those categories.
			List<AssessmentCategory> assessmentCategories; //ricardo, was AssessmentCategoryViewModel
			switch ((CategoryRandomisationType)assessment.categoryRandomisationTypeId) {
			case CategoryRandomisationType.Any:
                    // i.e. pick up any category and shuffle and pick any random order
				assessmentCategories = assessment.assessmentCategories.ToList ();
				break;
			case CategoryRandomisationType.FromDefinedAndRespectOrder:
                    // ok, so this assessment has been defined with a subset of categories and we want the questions
                    // to follow the order in which the categories have been defined
				assessmentCategories = assessment.assessmentCategories.OrderBy (x => x.ordinalPosition).ToList ();
				break;
			case CategoryRandomisationType.FromDefinedAndIgnoreOrder:
                    // So similar to the previous one, this assessment has a subset of categories.
                    // however in this case, we should shuffle that subset and present questions randomly.
				assessmentCategories = assessment.assessmentCategories.Shuffle ().ToList ();
				break;
			default:
                    // aaahhh, I don't know what to do !
				throw new GeneratorException (string.Format (
					"Unrecognised value for CategoryRandomisationTypeId ({0}) on AssessmentViewModel (AssessmentId: {1})",
					assessment.categoryRandomisationTypeId, assessment.assessmentId));
			}

			// OK so now will have the categories and we have laid them in the correct order (see above)
			foreach (var assessmentCategory in assessmentCategories) {
				// lets setup the StudentAssessmentCategories collection

				// if category is empty then we will randomly pick one. This will (should) occur when the category randomisation has been
				// set to ANY (see above).
				var newStudentAssessmentCategory = new AssessmentSubmissionCategory {
					categoryName = assessmentCategory.category,
					proficiencyLevelId = assessmentCategory.proficiencyLevelId,
					proficiencyLevelName = assessmentCategory.proficiencyLevelName,
					categoryId = assessmentCategory.categoryId == 0 ? Categories.PickRandom (1).First ().nodeCategoryId : assessmentCategory.categoryId,
				};

				// Just in case the above has picked the same category and proficiency level, we should ignore and continue
				if (studentAssessment.studentAssessmentCategories.Any (x =>
			        x.proficiencyLevelId == newStudentAssessmentCategory.proficiencyLevelId &&
				    x.categoryId == newStudentAssessmentCategory.categoryId)) {
					Debug.Log (string.Format (
						"Assessment.Generator warning: Cateory ID {0} and ProficiencyLevel ID {1} has already been added to this assessment. As such, the duplicate will be skipped",
						newStudentAssessmentCategory.categoryId,
						newStudentAssessmentCategory.proficiencyLevelId
					));
					continue;
				}

				// so before we can calculate the Max Achievable score for this category and proficiency level,
				// we need to first create our collection of questions. There are rules on how to do this based
				// on the finite set of QuestionTypes.
				// So, each set of "category and proficiency" dictates how to randomise the QuestionTypes
				// and how many questions of that type to pick up.
				// The logic below will first determine how to randomise the QuestionTypes and then, will create
				// a collection of questions (the number of which is defined by the TotalQuestions field) to act as placeholders for
				// the actual questions. The questions will be picked up later.
				var questionTypes = new List<AssessmentCategoryQuestionType> ();  //was AssessmentCategoryQuestionTypeViewModel
				switch ((QuestionTypeRandomisationType)assessmentCategory.questionTypeRandomisationTypeId) {
				case QuestionTypeRandomisationType.Any:
                        // So, pick up any Question Type in any random order.
                        // So, to make life easier next, we will simply place as many "AssessmentCategoryQuestionType" records in the
                        // collection as there are "TotalQuestions" defined. For instance, if we need to randomly
                        // select 10 questions from any type, we will create 10 records where the QuestionType for
                        // each of those 10 will be randomly picked. Finally, for each "AssessmentCategoryQuestionType" we only want
                        // to pick up 1 question
					for (var i = 0; i < assessmentCategory.totalQuestions; i++) {
						questionTypes.Add (new AssessmentCategoryQuestionType { //was AssessmentCategoryQuestionTypeViewModel
							questionType = _questionTypeIds.PickRandom (1).First (),
							ordinalPosition = i,
							totalQuestions = 1
						});
					}
					break;
				case QuestionTypeRandomisationType.FromDefinedAndRespectOrder:
                        // OK, so for this particular Category and Proficiency Level, the set of QuestionTypes has be defined
                        // and we need to present questions in that exact order specified.
					questionTypes = assessmentCategory.assessmentCategoryQuestionTypes
                                .OrderBy (x => x.ordinalPosition).ToList ();
					break;
				case QuestionTypeRandomisationType.FromDefinedAndIgnoreOrder:
                        // In this case, this particular category and proficiency level dictates the set of QuestionTypes to use
                        // however we must randomly choose the order (i.e. shuffle the deck)
					questionTypes = assessmentCategory.assessmentCategoryQuestionTypes
                                .Shuffle ().ToList ();
					break;
				default:
					throw new GeneratorException (string.Format (
						"Unrecognised value for QuestionTypeRandomisationTypeId ({0}) on AssessmentCategoryViewModel (CategoryId: {1}, Proficiency Level: {2})",
						assessmentCategory.questionTypeRandomisationTypeId, assessmentCategory.categoryId,
						assessmentCategory.proficiencyLevelId));
				}
                
				// So at this point, we have our QuestionTypes defined and ready to go in the correct order for this particular
				// Category and Proficiency Level. Now we need to iterate through each QuestionType and pickup as many questions
				// as each dictates.
			    foreach (var questionType in questionTypes) {
			        var questionsCreatedCount = 0;

                    // Up until now, the Assessment module has dictated what questions to get.
                    // i.e. Based on Category, Proficiency Level and Question Type
                    // Now, the Q&A module has the pool of questions and question templates which
                    // are linked to various Categories, Proficiency Levels and Question Types.

                    // For simplicity sakes, we shall refer to all questions in the pool as
                    // "question templates".
                    // Now, some questions in the pool may be straight out defined with fixed choices, answers and text.
                    // However, some questions can be templates which means, there is a set of instructions that dictate
                    // how to pick up choices, answers and which nodes are related. For these types of questions, this engine
                    // needs to construct the question from template.

                    // So... fixed questions takes priority and any remaining questions will be generated from the templates.

                    // Oh and one last point... we cannot have duplicate questions. So whilst it is possible
                    // to randomly pick up the same question, we must test and discard any duplicates.

                    // So the first thing we need to do is dip into the pool and pull out the "fixed"
                    // Questions which match the Category, Proficiency Level and Question Type
                    var questionsFixed = _questionPool
				        .Where(x => x.questionTypeId == questionType.questionType)
				        .Where(x => x.randomAnswerTypeId == (long)AnswerTypeEnum.NoIdecide)
				        .Where(x => x.questionCategories.Select(y => y.categoryId).Contains(newStudentAssessmentCategory.categoryId))
				        .Where(x => x.questionProficiencyLevels.Select(y => y.proficiencyLevelId).Contains(newStudentAssessmentCategory.proficiencyLevelId));

				    foreach (var question in questionsFixed)
				    {
				        // Have we reached the max number of questions allowed?
				        // ...if yes, jump out
				        if (questionsCreatedCount >= questionType.totalQuestions)
				            break;

				        var newStudentAssessmentQuestion = GenerateQuestion(questionType
				            , question
                            , newStudentAssessmentCategory
				            , questionOrdinalPosition);
				        if (newStudentAssessmentQuestion == null)
				            continue;

				        newStudentAssessmentCategory.studentAssessmentQuestions.Add(newStudentAssessmentQuestion);
				        questionOrdinalPosition++;
				        questionsCreatedCount++;
				    }

				    // Have we reached the max number of questions allowed for this type?
				    // ...if yes, jump out
				    if (questionsCreatedCount >= questionType.totalQuestions)
				        continue;
                    
                    // So since we need to generate more questions, let's now dip back into the pool
                    // and find the templated questions.
                    var questionsTemplated = _questionPool
                                            .Where (x => x.questionTypeId == questionType.questionType)
                                            .Where (x => x.randomAnswerTypeId != (long) AnswerTypeEnum.NoIdecide)
                                            .Where (x => x.questionCategories.Select (y => y.categoryId).Contains (newStudentAssessmentCategory.categoryId))
                                            .Where (x => x.questionProficiencyLevels.Select (y => y.proficiencyLevelId).Contains (newStudentAssessmentCategory.proficiencyLevelId))
                                            .ToList();

				    if (!questionsTemplated.Any())
				    {
				        Debug.Log(string.Format("Generator does not have any templated questions for QuestionType: {0} (Id: {1}) "
				            , questionType.questionTypeText
				            , questionType.questionType
				            ));
				        continue;
				    }

				    var infiniteLoopCounter = 0;
				    const int infiniteLoopBreaker = 100;
				    while (infiniteLoopCounter < infiniteLoopBreaker) {

                        // This infinite loop check is important because of the way we need to cycle the pool of questions templates
                        // in order to create a unique set of questions for this assessment.
                        // Consider the scenario where we want to randomly create 10 questions for the given
                        // category and proficiency level. If our pool of questions has only 5 but those five
                        // are templated questions (i.e. are configured to randomly select answers) then it is
                        // possible that we are going to iterate over and over again.
				        infiniteLoopCounter++;

				        // Have we reached the max number of questions allowed?
				        // ...if yes, we are done.
                        if (questionsCreatedCount >= questionType.totalQuestions)
					        break;

                        // so lets randomly pick the template... after all, it is possible that there
                        // are numerous templates chosen
				        var questionTemplate = questionsTemplated.Shuffle().First();
                        var newStudentAssessmentQuestion = GenerateQuestion(questionType
				                                                            , questionTemplate
				                                                            , newStudentAssessmentCategory
				                                                            , questionOrdinalPosition);
                        if(newStudentAssessmentQuestion == null)
                            continue;

                        newStudentAssessmentCategory.studentAssessmentQuestions.Add (newStudentAssessmentQuestion);
						questionOrdinalPosition++;
				        questionsCreatedCount++;
				    }

				    if (infiniteLoopCounter >= infiniteLoopBreaker)
				    {
				        Debug.Log(string.Format("Generator has tripped infiniteLoopCounter. QuestionType: {0}"
				            , questionType.questionTypeText));
				    }
                }

                // New, one last thing for this Category and Proficiency level... we need to calculate the Max Achievable score
                newStudentAssessmentCategory.maxAchievableScore =
                    newStudentAssessmentCategory.studentAssessmentQuestions.Sum (x => x.questionScore);

                //HIZI: Finally, shuffle all questions in the category.
                newStudentAssessmentCategory.studentAssessmentQuestions = newStudentAssessmentCategory.studentAssessmentQuestions.Shuffle().ToList();
                
				studentAssessment.studentAssessmentCategories.Add (newStudentAssessmentCategory);
			}

			// Go off and get the messages to display for the assessment
			studentAssessment.assessmentScores = assessment.assessmentScores;

			// The last thing to do now is to go back and re-calculate the total questions and max achievable scores
			studentAssessment.maxAchievableScoreValue =
                studentAssessment.studentAssessmentCategories.Sum (x => x.maxAchievableScore);

			return studentAssessment;
		}

	    private AssessmentSubmissionQuestion GenerateQuestion(
	          AssessmentCategoryQuestionType questionType
            , Question questionTemplate
	        , AssessmentSubmissionCategory newStudentAssessmentCategory
            , int questionOrdinalPosition
            )
	    {
            // OK, let's set up the StudentAssessmentQuestion record with some simple information
            // text, position, question id, score, etc
            var newStudentAssessmentQuestion = new AssessmentSubmissionQuestion
            {
                questionId = questionTemplate.questionId,
                ordinalPosition = questionOrdinalPosition,
                questionTypeId = questionType.questionType,
                questionTypeText = questionType.questionTypeText,
                questionText = questionTemplate.questionText,
                questionScore = !questionTemplate.questionAttemptScores.Any()
                    ? 0
                    : questionTemplate.questionAttemptScores.OrderBy(x => x.attemptNo).First().score,
                questionAttemptScores = questionTemplate.questionAttemptScores,
                questionProperties = new List<QuestionProperty>(questionTemplate.questionProperties)
            };
            // Important Note: each question has a set of Attempt Scores. Basically, this means that students can have numerous attempts
            // at getting the answer correct. However each attempt comes with a lesser score. The rules for how much to score at each
            // attempt is defined in the AtemptScores collection

            // a. First we need to determine the answer. In some cases the answer has been predefined but in the case of a templated question
            //    the answer may well be a randomisation base on Category and/or Proficiency Level
            //    Note:
            //         There is a situation where some parts of the 3D module are not actually touchable/clickable. In other words there is
            //         no mesh or colider or anything that allows the user to physically interact with it. As such, if the question type is
            //         a "touch the part", there is no point in selecting an answer node which cannot be touched. Hence you will see logic
            //         below which filters out any nodes which are flagged as not touchable when the question type is QuestionTypeEnum.IdentifyThePart
            switch ((AnswerTypeEnum)questionTemplate.randomAnswerTypeId)
            {
                case AnswerTypeEnum.ByCategory:
                    // randomly pick a node that matches the category of this question
                    var nodeA = _nodes.Where(x =>    questionType.questionType != (int) QuestionTypeEnum.IdentifyThePart
                                                  || LearnDataExtensions.IsNodeTouchable(x))
                                      .Where(x => x.nodeCategories.Select(nc => nc.nodeCategoryId)
                                                                  .Contains(newStudentAssessmentCategory.categoryId))
                                              .PickRandom(1)
                                              .FirstOrDefault();
                    if (nodeA != null)
                    {
                        newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Add(new AssessmentSubmissionQuestionOption
                        {
                            nodeId = nodeA.nodeId,
                            nodeName = nodeA.name,
                            ordinalPosition = 1,
                            isCorrect = true
                        });
                    }
                    break;

                case AnswerTypeEnum.ByProficiencyLevel:
                    // randomly pick a node that matches the proficiency level of this question
                    var nodeB = _nodes.Where(x => questionType.questionType != (int)QuestionTypeEnum.IdentifyThePart
                                                  || LearnDataExtensions.IsNodeTouchable(x))
                                      .Where(x => x.nodeProficiencyLevels.Select(nc => nc.proficiencyLevelId).Contains(newStudentAssessmentCategory.proficiencyLevelId))
                                              .PickRandom(1)
                                              .FirstOrDefault();
                    if (nodeB != null)
                    {
                        newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Add(new AssessmentSubmissionQuestionOption
                        {
                            nodeId = nodeB.nodeId,
                            nodeName = nodeB.name,
                            ordinalPosition = 1,
                            isCorrect = true
                        });
                    }
                    break;

                case AnswerTypeEnum.ByCategoryAndProficiencyLevel:
                    // randomly pick a node that matches both the category and proficiency level of this question
                    var nodeC = _nodes.Where(x => questionType.questionType != (int)QuestionTypeEnum.IdentifyThePart
                                                  || LearnDataExtensions.IsNodeTouchable(x))
                                      .Where(x => x.nodeProficiencyLevels.Select(nc => nc.proficiencyLevelId).Contains(newStudentAssessmentCategory.proficiencyLevelId))
                                              .Where(x => x.nodeCategories.Select(nc => nc.nodeCategoryId).Contains(newStudentAssessmentCategory.categoryId))
                                              .PickRandom(1)
                                              .FirstOrDefault();
                    if (nodeC != null)
                    {
                        newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Add(new AssessmentSubmissionQuestionOption
                        {
                            nodeId = nodeC.nodeId,
                            nodeName = nodeC.name,
                            ordinalPosition = 1,
                            isCorrect = true
                        });
                    }
                    break;
                case AnswerTypeEnum.NoIdecide:

                    // So the answer has been specified so simply lay it down
                    questionTemplate.questionAnswers.ToList().ForEach(qa => {
                        newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Add(new AssessmentSubmissionQuestionOption
                        {
                            nodeId = qa.nodeId,
                            nodeName = qa.nodeName,
                            optionText = qa.QuestionText,
                            ordinalPosition = 1,
                            isCorrect = true
                        });
                    });

                    break;
                default:
                    throw new GeneratorException(string.Format(
                        "Unrecognised value for RandomAnswerTypeId ({0}) on the QuestionViewModel (Question ID: {1})",
                        questionTemplate.randomAnswerTypeId, questionTemplate.questionId));
            }

            // b. Now we identify what the incorrect options are for this question. Either they have been defined for us explicitly,
            //    or, as this is a templated questions, there is a rule for randomly picking up incorrect option nodes. This rule is driven by the
            //    Category and/or the Proficiency Level as all Nodes are also classified under these options. Furthermore, if this is a templated
            //    question, we are also told how many incorrect options to choose.
            switch ((AnswerTypeEnum)questionTemplate.randomIncorrectAnswerTypeId)
            {
                case AnswerTypeEnum.ByCategory:
                    // So, we want to randomly pick up nodes which are in the same category
                    // of course we don't want to pick up the node which is the correct answer
                    for (var i = 0; i < questionTemplate.maximumNumberOfIncorrectOptions; i++)
                    {
                        var node = _nodes.Where(x => x.nodeCategories.Select(nc => nc.nodeCategoryId).Contains(newStudentAssessmentCategory.categoryId))
                                                 //.Where(x => !questionTemplate.QuestionAnswers.Select(qa => qa.NodeId).Contains(x.nodeId))
                                                 .Where(x => !newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Select(qo => qo.nodeId).Contains(x.nodeId)) // ignore any nodes already picked up as options
                                                 .ToList()
                                                 .PickRandom(1)
                                                 .FirstOrDefault();

                        // if we haven't found any incorrect options, there is no point in continuing with this loop
                        // because every other iteration will render the same null result
                        if (node == null)
                            break;

                        // check that we haven't already picked up this node... we don't want duplicate incorrect options
                        if (newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Select(x => x.nodeId).Contains(node.nodeId))
                            continue;

                        newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Add(new AssessmentSubmissionQuestionOption
                        {
                            nodeId = node.nodeId,
                            nodeName = node.name,
                            ordinalPosition = i,
                            isCorrect = false
                        });
                    }
                    break;
                case AnswerTypeEnum.ByProficiencyLevel:
                    // Same as above but this time we only care about proficiency level
                    for (var i = 0; i < questionTemplate.maximumNumberOfIncorrectOptions; i++)
                    {
                        var node = _nodes.Where(x => x.nodeProficiencyLevels.Select(nc => nc.proficiencyLevelId).Contains(newStudentAssessmentCategory.proficiencyLevelId))
                                    //.Where(x => !questionTemplate.QuestionAnswers.Select(qa => qa.NodeId).Contains(x.nodeId)) // ignore the actual answer
                                    .Where(x => !newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Select(qo => qo.nodeId).Contains(x.nodeId)) // ignore any nodes already picked up as options
                                    .ToList()
                                    .PickRandom(1)
                                    .FirstOrDefault();

                        // if we haven't found any incorrect options, there is no point in continuing with this loop
                        // because every other iteration will render the same null result
                        if (node == null)
                            break;

                        // check that we haven't already picked up this node... we don't want duplicate incorrect options
                        if (newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Select(x => x.nodeId).Contains(node.nodeId))
                            continue;

                        newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Add(new AssessmentSubmissionQuestionOption
                        {
                            nodeId = node.nodeId,
                            nodeName = node.name,
                            ordinalPosition = i,
                            isCorrect = false
                        });
                    }
                    break;
                case AnswerTypeEnum.ByCategoryAndProficiencyLevel:
                    // same as above but we want to isolate both Category and Proficiency Levels
                    for (var i = 0; i < questionTemplate.maximumNumberOfIncorrectOptions; i++)
                    {
                        var node = _nodes
                                    .Where(x => x.nodeCategories.Select(nc => nc.nodeCategoryId).Contains(newStudentAssessmentCategory.categoryId))
                                    .Where(x => x.nodeProficiencyLevels.Select(nc => nc.proficiencyLevelId).Contains(newStudentAssessmentCategory.proficiencyLevelId))
                                    .Where(x => !newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Select(qo => qo.nodeId).Contains(x.nodeId)) // ignore any nodes already picked up as options
                                    .ToList()
                                    .PickRandom(1)
                                    .FirstOrDefault();

                        // if we haven't found any incorrect options, there is no point in continuing with this loop
                        // because every other iteration will render the same null result
                        if (node == null)
                            break;

                        // check that we haven't already picked up this node... we don't want duplicate incorrect options
                        if (newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Select(x => x.nodeId).Contains(node.nodeId))
                            continue;

                        newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Add(new AssessmentSubmissionQuestionOption
                        {
                            nodeId = node.nodeId,
                            nodeName = node.name,
                            ordinalPosition = i,
                            isCorrect = false
                        });
                    }
                    break;
                case AnswerTypeEnum.NoIdecide:

                    // so in this case, the options have been pre-defined

                    // This field is available to be set in config. However in the event that
                    // it has been left, then the assumption will be to show all the options.
                    var maxNumberOfOptions = questionTemplate.maximumNumberOfIncorrectOptions;
                    if (maxNumberOfOptions == 0)
                        maxNumberOfOptions = questionTemplate.questionIncorrectOptions.Count;

                    questionTemplate.questionIncorrectOptions.Shuffle().ToList().ForEach(qio =>
                    {
                        if (newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Count(x=>!x.isCorrect) >=
                            maxNumberOfOptions)
                            return;

                        newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Add(new AssessmentSubmissionQuestionOption
                        {
                            nodeId = qio.nodeId,
                            nodeName = qio.nodeName,
                            optionText = qio.optionText,
                            ordinalPosition = qio.ordinalPosition,
                            isCorrect = false
                        });
                    });
                    break;
            }

            // c. Now we need to shuffle the options. If the correct answer is always at the top of the list, very
            //    soon the students will discover the pattern and always be quessing correctly
            var index = 0;
            newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Shuffle().ToList().ForEach(option => {
                option.ordinalPosition = index++;
            });

            // d. Need to lay down the messages for Right and Wrong answers
            var correctMessageProperty = questionTemplate.questionProperties.FirstOrDefault(x =>
              x.propertyKey.Equals("CorrectMessage", StringComparison.OrdinalIgnoreCase));
            if (correctMessageProperty != null)
            {
                newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Where(x => x.isCorrect).ToList().ForEach(
                    correctOption => {
                        correctOption.messageToShow = correctMessageProperty.propertyValue;
                    });
            }

            var incorrectMessageProperty = questionTemplate.questionProperties.FirstOrDefault(x =>
              x.propertyKey.Equals("IncorrectMessage", StringComparison.OrdinalIgnoreCase));
            if (incorrectMessageProperty != null)
            {
                newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Where(x => !x.isCorrect).ToList().ForEach(
                    incorrectOption => {
                        incorrectOption.messageToShow = incorrectMessageProperty.propertyValue;
                    });

                // in the event that we dont have an incorrect option, we want to throw in a dummy
                // one so that the message can be shown to the user
                if (newStudentAssessmentQuestion.studentAssessmentQuestionOptions.All(x => x.isCorrect))
                {
                    newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Add(new AssessmentSubmissionQuestionOption
                    {
                        messageToShow = incorrectMessageProperty.propertyValue
                    });
                }
            }

            // So we should now shuffle the options. If we don't, then correct option will always be the first one
            // and this is not a qood thing
            newStudentAssessmentQuestion.studentAssessmentQuestionOptions = newStudentAssessmentQuestion
                .studentAssessmentQuestionOptions
                .Shuffle()
                .ToList();

            // e. Related nodes... so these are parts of the 3D model which need to be turned on and either highlighted or made transparent.
            //    The related nodes are relevant to the question and acts as a critical visual cue to the student
            //    The characterstics of the node are defined in the properties collections.
            //    Related nodes can either be predefined or can be templated based upon the answer node.
            switch ((RelatedPartTypeEnum)questionTemplate.relatedPartTypeId)
            {
                case RelatedPartTypeEnum.ParentOfAnswer:
                    // So the related node will actually be the parent node of the answer (assuming the answer option has a node)
                    questionTemplate.questionAnswers.ToList().ForEach(qa => {
                        if (qa.nodeId == 0)
                            return;
                        if (!qa.ParentNodeId.HasValue)
                        {
                            // try geting the parent node Id from the node collection
                            var node = _nodes.FirstOrDefault(x => x.nodeId == qa.nodeId);
                            if (node == null)
                                return;
                            if (!node.ParentNodeId.HasValue)
                                return;
                            qa.ParentNodeId = node.ParentNodeId.Value;
                        }

                        var parentNode = _nodes.FirstOrDefault(x => x.nodeId == qa.ParentNodeId.Value);
                        if (parentNode == null)
                            return;

                        newStudentAssessmentQuestion.studentAssessmentQuestionRelatedNodes.Add(new AssessmentSubmissionQuestionRelatedNode
                        {
                            nodeId = parentNode.nodeId,
                            nodeName = parentNode.name,
                            isHighlighted = false,
                            isTransparent = false
                        });

                    });
                    break;

                case RelatedPartTypeEnum.GrandParentOfAnswer:
                    // So the related node will actually be the grand parent node of the answer (assuming the answer option has a node)
                    questionTemplate.questionAnswers.ToList().ForEach(qa => {
                        if (qa.nodeId == 0)
                            return;
                        if (!qa.ParentNodeId.HasValue)
                        {
                            // try getting the parent node Id from the node collection
                            var node = _nodes.FirstOrDefault(x => x.nodeId == qa.nodeId);
                            if (node == null)
                                return;
                            if (!node.ParentNodeId.HasValue)
                                return;
                            qa.ParentNodeId = node.ParentNodeId.Value;
                        }

                        var parentNode = _nodes.FirstOrDefault(x => x.nodeId == qa.ParentNodeId.Value);
                        if (parentNode == null)
                            return;
                        if (!parentNode.ParentNodeId.HasValue)
                            return;

                        var grandParentNode = _nodes.FirstOrDefault(x => x.nodeId == parentNode.ParentNodeId.Value);
                        if (grandParentNode == null)
                            return;

                        newStudentAssessmentQuestion.studentAssessmentQuestionRelatedNodes.Add(new AssessmentSubmissionQuestionRelatedNode
                        {
                            nodeId = grandParentNode.nodeId,
                            nodeName = grandParentNode.name,
                            isHighlighted = false,
                            isTransparent = false
                        });

                    });
                    break;

                case RelatedPartTypeEnum.GreatGrandParentOfAnswer:
                    // So the related node will actually be the great grand parent node of the answer (assuming the answer option has a node)
                    questionTemplate.questionAnswers.ToList().ForEach(qa => {
                        if (qa.nodeId == 0)
                            return;
                        if (!qa.ParentNodeId.HasValue)
                        {
                            // try geting the parent node Id from the node collection
                            var node = _nodes.FirstOrDefault(x => x.nodeId == qa.nodeId);
                            if (node == null)
                                return;
                            if (!node.ParentNodeId.HasValue)
                                return;
                            qa.ParentNodeId = node.ParentNodeId.Value;
                        }

                        var parentNode = _nodes.FirstOrDefault(x => x.nodeId == qa.ParentNodeId.Value);
                        if (parentNode == null)
                            return;
                        if (!parentNode.ParentNodeId.HasValue)
                            return;

                        var grandParentNode = _nodes.FirstOrDefault(x => x.nodeId == parentNode.ParentNodeId.Value);
                        if (grandParentNode == null)
                            return;
                        if (!grandParentNode.ParentNodeId.HasValue)
                            return;

                        var greatGrandParentNode = _nodes.FirstOrDefault(x => x.nodeId == grandParentNode.ParentNodeId.Value);
                        if (greatGrandParentNode == null)
                            return;

                        newStudentAssessmentQuestion.studentAssessmentQuestionRelatedNodes.Add(new AssessmentSubmissionQuestionRelatedNode
                        {
                            nodeId = greatGrandParentNode.nodeId,
                            nodeName = greatGrandParentNode.name,
                            isHighlighted = false,
                            isTransparent = false
                        });

                    });
                    break;

                case RelatedPartTypeEnum.Specified:
                    // The related nodes have been predefined, so just pick them up and set
                    questionTemplate.questionRelatedNodes.ToList().ForEach(qrn => {
                        var qrnVm = new AssessmentSubmissionQuestionRelatedNode
                        {
                            nodeId = qrn.nodeId,
                            nodeName = qrn.nodeName,
                            isHighlighted = qrn.questionRelatedNodeProperties.Any(x => x.propertyKey.Equals(RelatedNodePropertyKeyIsHighlighted, StringComparison.OrdinalIgnoreCase)
                           && x.propertyValue.Equals("true", StringComparison.OrdinalIgnoreCase)),
                            isTransparent = qrn.questionRelatedNodeProperties.Any(x => x.propertyKey.Equals(RelatedNodePropertyKeyIsTransparent, StringComparison.OrdinalIgnoreCase)
                           && x.propertyValue.Equals("true", StringComparison.OrdinalIgnoreCase)),
                        };

                        // HACK: if Question is a "touch the part" and the related node
                        // is set to IsHighlighted=true, then the Related Node
                        // represents the ATOM in the 3D model which will be selected 
                        // so that it can become the centre of manipulation and all of its
                        // children and descendants will become touchable answers.
                        // Note: if in this case, the related node is also "IsTransparent=true"
                        // we recognise this as an unsupported situation and so we need to 
                        // change it to false
                        if (newStudentAssessmentQuestion.questionTypeId == (long)QuestionTypeEnum.IdentifyThePart
                               && qrnVm.isHighlighted)
                        {
                            qrnVm.isHighlighted = false;
                            qrnVm.isTransparent = false;
                        }

                        newStudentAssessmentQuestion.studentAssessmentQuestionRelatedNodes.Add(qrnVm);
                    });
                    break;
            }

            // f. Need to format the messages, questions and texts to please
            //    keywords if possible
            newStudentAssessmentQuestion.questionText = ParseText(
                newStudentAssessmentQuestion.questionText,
                newStudentAssessmentQuestion.studentAssessmentQuestionRelatedNodes.ToList(),
                newStudentAssessmentQuestion.studentAssessmentQuestionOptions.ToList(),
                newStudentAssessmentCategory,
                null
            );
            
            if(newStudentAssessmentCategory.studentAssessmentQuestions == null)
                newStudentAssessmentCategory.studentAssessmentQuestions = new List<AssessmentSubmissionQuestion>();

            // STOP !
            // I know we have done all this work up until now just for one question !!! phew !!!
            // However, it is IMPORTANT that we do not duplicate any questions.
            // So if by chance, we've already picked up the same question... we need to discard it.
            // A duplicate means that BOTH the question text and the correct answer are the same.
            // It is therefore possible that multiple questions will ask the same thing (i.e. same text) 
            // however the answer can be different.
            var existingQuestionsWithSameText = newStudentAssessmentCategory.studentAssessmentQuestions
                .Where(x => x.questionText.Equals(newStudentAssessmentQuestion.questionText, StringComparison.OrdinalIgnoreCase));
            var questionAlreadyExists = false;

            foreach (var existingQuestionWithSameText in existingQuestionsWithSameText)
            {
                // so for each question which has already been added to the assessment that has the same text as
                // this one we are creating now, look at all the correct options...
                var existingQuestionCorrectOptions =
                    existingQuestionWithSameText.studentAssessmentQuestionOptions.Where(x => x.isCorrect);

                var sameCorrectOptionCount = 0;
                // we want to keep a count of how many of the correct answers in this new question match the existing question
                // so if lets say the existing question has 2 correct answers but the new questions only has 1 correct answer
                // then technically this is a different answer... perhaps this is a strange situation as for the exact same question text
                // one could have 2 answers and the other 1 answer...  however, if the question says identify the highlighted parts and in
                // one case it is showing 1 highlight part and the other is showing 2 highlighted parts, then again technically, this is a different
                // question where the text is the same but the answer count it different
                foreach (var existingQuestionCorrectOption in existingQuestionCorrectOptions)
                {
                    // ... so for each correct option, does it exist in this question that we are creating now

                    foreach (var newQuestionCorrectOption in newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Where(x => x.isCorrect))
                    {
                        if (newQuestionCorrectOption.nodeId != 0
                            && existingQuestionCorrectOption.nodeId != 0
                            && newQuestionCorrectOption.nodeId == existingQuestionCorrectOption.nodeId)
                        {
                            sameCorrectOptionCount++;
                            continue;
                        }

                        if (newQuestionCorrectOption.optionText != null
                            && existingQuestionCorrectOption.optionText != null
                            && newQuestionCorrectOption.optionText.Equals(existingQuestionCorrectOption.optionText, StringComparison.OrdinalIgnoreCase))
                        {
                            sameCorrectOptionCount++;
                            continue;
                        }
                    }
                }

                // OK... you might be thinking that a question should only have one correct option right?
                // well if not then ignore this.
                // If you are, well perhaps for the moment, the questions don't have multiple correct answers,
                // but that is not to say it isn't possible... and in anycase, the data model supports it, so
                // why not lay down the work now to be prepared if/when it arrives.
                if (existingQuestionWithSameText.studentAssessmentQuestionOptions.Count(x => x.isCorrect) == sameCorrectOptionCount)
                {
                    questionAlreadyExists = true;
                }
            }

            if (questionAlreadyExists)
            {
                Debug.Log(string.Format(
                    "Assessment.Generator debug: Question ID {0} with text {1} has already been created for this assessment. As such, the duplicate will be ignored",
                    newStudentAssessmentQuestion.questionId, newStudentAssessmentQuestion.questionText));
                return null;
            }

            // OK, now we can resume and parse the option messages
            newStudentAssessmentQuestion.studentAssessmentQuestionOptions.ToList().ForEach(option => {
                option.messageToShow = ParseText(option.messageToShow,
                    newStudentAssessmentQuestion.studentAssessmentQuestionRelatedNodes.ToList(),
                    newStudentAssessmentQuestion.studentAssessmentQuestionOptions.ToList(),
                    newStudentAssessmentCategory, null);

                option.optionText = ParseText(option.optionText,
                    newStudentAssessmentQuestion.studentAssessmentQuestionRelatedNodes.ToList(),
                    newStudentAssessmentQuestion.studentAssessmentQuestionOptions.ToList(),
                    newStudentAssessmentCategory, null);
            });

            // STOP AGAIN !
            // If we don't have an answer... we don't have a question
            if (!newStudentAssessmentQuestion.studentAssessmentQuestionOptions.Any(x => x.isCorrect))
            {
                Debug.Log(string.Format(
                    "Assessment.Generator warning: Question ID {0} with text {1} does not have any options. As such, it will be ignored",
                    newStudentAssessmentQuestion.questionId,
                    newStudentAssessmentQuestion.questionText
                ));
                return null;
            }

	        return newStudentAssessmentQuestion;
	    }

        /// <summary>
        /// Call this method to get a list of Assessments to present on the screen along with the proficiency levels that
        /// are applicable to that assessment.
        /// </summary>
        /// <returns>
        /// A view model that holds the name of the assessment to display on screen and the list of proficiency levels available
        /// for selection. For each level, there is a unique Assessment ID which is needed for the <see cref="GenerateAssessment"/>
        /// method.
        /// </returns>
        public IEnumerable<AssessmentOptionViewModel> ListAssessmentOptions ()
		{
			var optionKey = 1000; // specifically set this to 1000 to indicate this is NOT the assessment Id, but rather a view model key
			var retList = (from assessment in (_assessments.OrderBy(x=> x.targetProficiencyLevel).GroupBy (x => new {x.assessmentName}))
                           let assessmentOptionKey = optionKey++
			                  select new AssessmentOptionViewModel {
				AssessmentOptionKey = assessmentOptionKey,
				AssessmentName = assessment.Key.assessmentName, //assessment.Key,

				ProficiencyLevels = from a in assessment
				                              select new AssessmentOptionViewModel.ProficiencyOptionViewModel {
					AssessmentId = a.assessmentId,
					AssessmentName = a.assessmentName,
					ProficiencyId = a.targetProficiencyLevelId,
					ProficiencyName = a.targetProficiencyLevel,
				}
			}).ToList ();
            
			return retList;
		}

		/// <summary>
		/// Initialises the Generator engine in preparation for <see cref="ListAssessmentOptions"/> and then <see cref="GenerateAssessment"/>.
		/// Note: the generator is created as a singleton.
		/// </summary>
		/// <param name="nodesFlattenedCollection">We want a collection of nodes which are not in hierarchical order</param>
		/// <param name="assessments">collection of assessments which have been sent down from the server</param>
		/// <param name="questionTemplates">collection of questions and question templates sent down from the server</param>
		/// <param name="singleton">The generator can be initialised as a singleton class or instance. Default is FALSE</param>
		/// <returns></returns>
		public static Generator Init (
			IEnumerable<LearnNode> nodesFlattenedCollection,
			IEnumerable<Assessment> assessments, // ricardo, used to be AssessmentViewModel
			IEnumerable<Question> questionTemplates,
			bool singleton = false)
		{
			var gen = new Generator {
				_nodes = nodesFlattenedCollection,
				_assessments = assessments,
				_questionPool = questionTemplates
			};

			if (!singleton)
				return gen;

			if (_singleton != null)
				return _singleton;

			_singleton = gen;

			return _singleton;

		}

		private static Generator _singleton;

		private List<NodeCategory> Categories {
			get {
				if (_categories.Any ())
					return _categories;

				foreach (var node in _nodes) {
					foreach (var nodeCategoryViewModel in node.nodeCategories) {
						if (_categories.All (x => x.nodeCategoryId != nodeCategoryViewModel.nodeCategoryId))
							_categories.Add (nodeCategoryViewModel);
					}
				}

				return _categories;
			}
		}

		private List<ProficiencyLevel> ProficiencyLevels {
			get {
				if (_proficiencyLevels != null)
					return _proficiencyLevels;

				_proficiencyLevels = new List<ProficiencyLevel> ();

				foreach (var assessmentViewModel in _assessments) {
					foreach (var assessmentCategory in assessmentViewModel.assessmentCategories) {
						if (_proficiencyLevels.Any (x => x.Id == assessmentCategory.proficiencyLevelId))
							continue;

						_proficiencyLevels.Add (new ProficiencyLevel {
							Id = assessmentCategory.proficiencyLevelId,
							Name = assessmentCategory.proficiencyLevelName
						});
					}
				}

				return _proficiencyLevels;
			}
		}

		private readonly List<long> _questionTypeIds;
		private readonly List<NodeCategory> _categories;
		private  List<ProficiencyLevel> _proficiencyLevels;
		private IEnumerable<LearnNode> _nodes;
		private IEnumerable<Assessment> _assessments;
		// ricardo, used to be AssessmentViewModel
		private IEnumerable<Question> _questionPool;

		private Generator ()
		{
			_categories = new List<NodeCategory> ();
			_questionTypeIds = new List<long> {
				(long)QuestionTypeEnum.IdentifyThePart,
				(long)QuestionTypeEnum.NameThePart,
				(long)QuestionTypeEnum.MultipleChoiceQuestion,
			};
		}

		public enum CategoryRandomisationType
		{
			Any = 8000,
			FromDefinedAndRespectOrder = 8001,
			FromDefinedAndIgnoreOrder = 8002
		}

		public enum QuestionTypeRandomisationType
		{
			Any = 7000,
			FromDefinedAndRespectOrder = 7001,
			FromDefinedAndIgnoreOrder = 7002
		}

		public enum AnswerTypeEnum
		{
			ByCategory = 6001,
			ByProficiencyLevel = 6002,
			ByCategoryAndProficiencyLevel = 6003,
			NoIdecide = 6004
		}

		public enum QuestionTypeEnum
		{
			IdentifyThePart = 5001,
			NameThePart = 5002,
			MultipleChoiceQuestion = 5003
		}

		public enum RelatedPartTypeEnum
		{
			ParentOfAnswer = 4001,
			GrandParentOfAnswer = 4002,
			GreatGrandParentOfAnswer = 4003,
			Specified = 4004
		}

		public const string RelatedNodePropertyKeyIsHighlighted = "IsHighlighted";
		public const string RelatedNodePropertyKeyIsTransparent = "IsTransparent";

		private class ProficiencyLevel
		{
			public long Id { get; set; }

			public string Name { get; set; }
		}
	}

	public class GeneratorException : Exception
	{
		public GeneratorException (string message) : base (message)
		{

		}
	}
}
