using AI_Diet.Models;
using AI_Diet.Models.UserModels;
using System.Text;

namespace AI_Diet.Services
{
    public class ChatRequestBuilder : IChatRequestBuilder
    {
        private string _messageBegining = "{\"model\": \"gpt-3.5-turbo\",\"messages\":[{ ";

        public string BuildDietRequest(DietRequestModel dietRequestModel)
        {
            var foodDetails = dietRequestModel.FoodDetails;
            var dietRequest = new StringBuilder(_messageBegining);

            dietRequest.AppendLine("\"role\": \"system\", \"content\": \"Ти персональний дієтолог\"},");
            BuildBaseMessage(dietRequestModel.DietData, dietRequest);
            dietRequest.AppendLine("Мені потрібно щоб ти розписав мені дієту згідно моїх параметрів.");

            if (IsAnyFoodDetails(foodDetails))
            {
                AppendFoodDetails(foodDetails, dietRequest);
            }
            dietRequest.Append("Дієту розпиши наче ти персональний дієтолог, і розписуєш дієту своєму клієнту у вигляді документу\"}");

            return dietRequest.ToString();
        }

        public string BuildTrainingRequest(DietData dietData)
        {
            var trainingRequest = new StringBuilder(_messageBegining);

            trainingRequest.AppendLine("\"role\": \"system\", \"content\": \"Ти персональний тренер\"},");
            BuildBaseMessage(dietData, trainingRequest);
            trainingRequest.AppendLine("Мені потрібно, щоб ти розписав мені персональні тренування у спортивному залі, " +
                "по дням, для досягнення моєї цілі. Тренування розпиши наче ти персональний тренер, " +
                "і розписуєш тренування своєму клієнту у вигляді документу");

            return trainingRequest.ToString();
        }

        private void BuildBaseMessage(DietData dietData, StringBuilder request)
        {
            var goal = AdjustGoalToString(dietData.Goal);
            var gender = AdjustGenderToString(dietData.Gender);

            request.AppendLine($"{{ \"role\": \"user\", \"content\": \"" +
                $"Я {gender} мені {dietData.Age} років, мій зріст {dietData.Height} сантиметрів, " +
                $"а моя вага складає {dietData.Weight} кілограм. Мою денну фізичну активність можна описати так: {dietData.PhysicalActivity}," +
                $"а моя ціль: {goal}.");
        }

        private string AdjustGoalToString(Enums.DietGoal goal)
        {
            switch (goal)
            {
                case Enums.DietGoal.LooseWeight:
                    return "Похудати";
                case Enums.DietGoal.KeepWeight:
                    return "Зберегти масу";
                case Enums.DietGoal.GainWeight:
                    return "Набрати м'язову масу";
                default:
                    return "Невідома";
            }
        }

        private string AdjustGenderToString(Enums.Gender gender)
        {
            switch (gender)
            {
                case Enums.Gender.Male:
                    return "чоловік";
                case Enums.Gender.Female:
                    return "жінка";
                default:
                    return "неідентифікований";
            }
        }

        private void AppendFoodDetails(FoodDetails dietData, StringBuilder dietRequest)
        {
            dietRequest.AppendLine("Але, будь ласка, зверни увагу на деякі деталі, щодо майбутньої дієти:");

            if (!string.IsNullOrEmpty(dietData.Allergies))
            {
                dietRequest.AppendLine($"* У мене є алергія на наступне: {dietData.Allergies}.");
            }
            if (!string.IsNullOrEmpty(dietData.FoodRestrictions))
            {
                dietRequest.AppendLine($"* У мене є обмеження у споживанні: {dietData.FoodRestrictions}.");
            }
            if (!string.IsNullOrEmpty(dietData.DislikeFood))
            {
                dietRequest.AppendLine($"* Мені не подобається наступна їжа: {dietData.DislikeFood}, " +
                    $"тому при побудові дієти використовуй її по мінімум.");
            }
            if (!string.IsNullOrEmpty(dietData.FoodPreferences))
            {
                dietRequest.AppendLine($"* Я люблю наступну їжу: {dietData.FoodPreferences}.");
            }
        }

        private bool IsAnyFoodDetails(FoodDetails dietData)
        {
            if (dietData != null &&
                (!string.IsNullOrEmpty(dietData.DislikeFood) ||
                !string.IsNullOrEmpty(dietData.FoodRestrictions) ||
                !string.IsNullOrEmpty(dietData.FoodPreferences) ||
                !string.IsNullOrEmpty(dietData.Allergies)))
            {
                return true;
            }
            return false;
        }
    }
}
