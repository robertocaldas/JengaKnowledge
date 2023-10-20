using System;
using UnityEngine;

namespace GTSchool.Assessment.KnowledgeJenga
{
    [Serializable]
    public class KnowledgeBlock
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private string subject;
        [SerializeField]
        private string grade;
        [SerializeField]
        private int mastery;
        [SerializeField]
        private string domainid;
        [SerializeField]
        private string domain;
        [SerializeField]
        private string cluster;
        [SerializeField]
        private string standardid;
        [SerializeField]
        private string standarddescription;

        public int Id => id;
        public string Subject => subject;
        public string Grade => grade;
        public int Mastery => mastery;
        public string DomainId => domainid;
        public string Domain => domain;
        public string Cluster => cluster;
        public string StandardId => standardid;
        public string StandardDescription => standarddescription;
    }
}