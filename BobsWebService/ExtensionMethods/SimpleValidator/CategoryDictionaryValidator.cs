namespace BobsWebService.ExtensionMethods.SimpleValidator
{
    public static class CategoryDictionaryValidator
    {
        public static int MAX_LENGTH = 300;

        public static bool IsValid(this Dictionary<string, string> categoryDictionary)
        {
            bool isValid = true;

            foreach(var kvp in categoryDictionary) 
            {
                if(kvp.Key == null)
                {
                    isValid = false;
                    break;
                }

                if(kvp.Value != null ) 
                {
                    if(kvp.Value.Length > MAX_LENGTH)
                    {
                        isValid = false;
                        break;
                    }
                }

                if(kvp.Key.Length > MAX_LENGTH)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }
    }
}
