using CustomUtilities.Attributes;
using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/EPOCH Time Persistent")]
    public class DBEpochTime : DBInt
    {
        const int OneDayEpochValue = 86400;

        private void OnEnable()
        {
            ((IDBVariable)this).Load();
        }

        [Button]
        private void AddDay()
        {
            ApplyChange(OneDayEpochValue);
        }

        [Button]
        private void SubtractDay()
        {
            ApplyChange(-OneDayEpochValue);
        }

        [Button]
        private void AddDays(int days)
        {
            for (int i = 0; i < days; i++)
            {
                AddDay();
            }
        }

        [Button]
        private void SubtractDays(int days)
        {
            for (int i = 0; i < days; i++)
            {
                SubtractDay();
            }
        }
    }
}
