using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Dialogue;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogue
{
    public static class Helpers
    {        
        public static void SplitText()
        {
            var totalLine =
                "Text still spans upto two Lines at a time. <color=red>Accentuated text</color> can be dropped to an alternate color like so. This will allow for many more characters per text 'chunk'. 96 characters to a row currently has nice spacing.";

            // string line2 = "Text still spans upto two Lines at a time. Accentuated text can be dropped to an alternate color like so.";
            // string line3 = "This will allow for many more characters per text 'chunk'. 96 characters to a row currently has nice spacing.";       
            var regex = new Regex(@"<.*?>");
            totalLine = regex.Replace(totalLine, string.Empty);
        }
 

        [ContextMenu("Sort By Name")]
        public static Sprite[] SortByName(Sprite[] sprites)
        {
            var ordered = sprites.OrderBy(x => x.name).ToArray<Sprite>();
            return ordered;
        }

        private static IEnumerator TweenScale(this Transform transform, float scaleFactor = 1.1f, float duration = .33f)
        {
            float timer = 0;
            var oldScale = transform.localScale;

            while (timer < .2f)
            {
                transform.localScale = Vector3.Slerp(oldScale, oldScale * scaleFactor, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            while (timer < .2f)
            {
                transform.localScale = Vector3.Slerp(transform.localScale, oldScale, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            transform.localScale = oldScale;
            yield return null;
        }

    }
}