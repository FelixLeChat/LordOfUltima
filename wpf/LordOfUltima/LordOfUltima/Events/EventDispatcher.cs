using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordOfUltima
{
    class EventDispatcher
    {
        // List of possible actions
        public enum action
        {
            CANEVAS_CLICK = 1
        }
        // Instance of the class
        private static EventDispatcher m_ins = null;

        // All event and response functions
        private static Dictionary<int, List<Action<string>> > m_subcribers;


        /*
         * Constructeur
        */
        private EventDispatcher()
        {
            m_subcribers = new Dictionary<int,List<Action<string>>>();
        }

        /*
         * Implementation pour patron singleton
        */
        public static EventDispatcher getInstance()
        {
            if(m_ins == null)
            {
                m_ins = new EventDispatcher();
            }
            return m_ins;
        }
        /*
         * Fonction to add a function to call when an event arrive
        */
        public void subcribe(int action ,Action<string> callback)
        {
            if(!m_subcribers.ContainsKey(action))
            {
                m_subcribers.Add(action, new List<Action<string>>());
            }

            m_subcribers[action].Add(callback);
        }

        // TODO : ajout de traitement de tous les events
        public void dispatchEvents(int action)
        {
            foreach(Action<string> callback in m_subcribers[action])
            {
                callback("");
            }
        }
    }
}
