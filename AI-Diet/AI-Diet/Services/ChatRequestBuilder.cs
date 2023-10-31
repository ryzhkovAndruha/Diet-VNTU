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
            dietRequest.Append("Мені потрібно щоб ти розписав мені дієту згідно моїх параметрів.");

            if (IsAnyFoodDetails(foodDetails))
            {
                AppendFoodDetails(foodDetails, dietRequest);
            }
            dietRequest.Append("Дієту розпиши наче ти персональний дієтолог, і розписуєш дієту своєму клієнту у вигляді документу. " +
                "Потрібно розписати план харчування на тиждень, з різними стравами, але збереженням денного калоражу. " +
                "Документ напиши строгою, технічною мовою. Потрібно, щоб він включав в себе денний калораж клієнту, а також біля кожної " +
                "страви було розписано хоча б приблизна кількість її калорій. Ім'я в документі не вказуй\"}]}");

            var result = dietRequest.ToString();
            return result;
        }

        public string BuildTrainingRequest(DietData dietData)
        {
            var trainingRequest = new StringBuilder(_messageBegining);

            trainingRequest.Append("\"role\": \"system\", \"content\": \"Ти персональний тренер\"},");
            BuildBaseMessage(dietData, trainingRequest);
            trainingRequest.Append("Мені потрібно, щоб ти розписав мені персональні тренування у спортивному залі, " +
                "по дням, для досягнення моєї цілі. Тренування розпиши наче ти персональний тренер, " +
                "і розписуєш тренування своєму клієнту у вигляді документу\"}]}");

            return trainingRequest.ToString();
        }

        private void BuildBaseMessage(DietData dietData, StringBuilder request)
        {
            var goal = AdjustGoalToString(dietData.Goal);
            var gender = AdjustGenderToString(dietData.Gender);

            request.Append($"{{ \"role\": \"user\", \"content\": \"" +
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
            dietRequest.Append("Але, будь ласка, зверни увагу на деякі деталі, щодо майбутньої дієти:");

            if (!string.IsNullOrEmpty(dietData.Allergies))
            {
                dietRequest.Append($"* У мене є алергія на наступне: {dietData.Allergies}.");
            }
            if (!string.IsNullOrEmpty(dietData.FoodRestrictions))
            {
                dietRequest.Append($"* У мене є обмеження у споживанні: {dietData.FoodRestrictions}.");
            }
            if (!string.IsNullOrEmpty(dietData.DislikeFood))
            {
                dietRequest.Append($"* Мені не подобається наступна їжа: {dietData.DislikeFood}, " +
                    $"тому при побудові дієти використовуй її по мінімуму.");
            }
            if (!string.IsNullOrEmpty(dietData.FoodPreferences))
            {
                dietRequest.Append($"* Я люблю наступну їжу: {dietData.FoodPreferences}.");
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
