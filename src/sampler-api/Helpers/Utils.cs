using System.Reflection;

namespace sampler_api.Helpers
{
    public static class Utils
    {
        public static T Combine<T>(T A, T B)
        {
            foreach (PropertyInfo AProp in A.GetType().GetProperties())
            {
                if (AProp.GetValue(A) == null)
                {
                    PropertyInfo BProp = B.GetType().GetProperty(AProp.Name);
                    AProp.SetValue(A, BProp.GetValue(B));
                }
            }

            return A;
        }

    }
}