using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Form_Element
    {
        [FirestoreProperty]
        public string Element_ID { get; set; }
        [FirestoreProperty]
        public string Element_Value { get; set; }

    }
}