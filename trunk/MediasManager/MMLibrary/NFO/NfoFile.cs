﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaManager.Library.NFO
{
    public class NfoFile
    {
        public static NfoTV getNfoTV(String NfoPath)
        {
            NfoTV nf;
            Serializer s = new Serializer(NfoPath, new NfoTV());
            nf = (NfoTV)s.FromFile();
            return nf;
        }

        public static NfoMovie getNfoMovie(String NfoPath)
        {
            NfoMovie nf;
            Serializer s = new Serializer(NfoPath, new NfoMovie());
            nf = (NfoMovie)s.FromFile();
            return nf;
        }

        public static bool saveNfoTV(NfoTV nf, String NfoPath)
        {
            Serializer s = new Serializer(NfoPath, nf);
            return s.ToFile();
        }

        public static bool saveNfoMovie(NfoMovie nf, String NfoPath)
        {
            Serializer s = new Serializer(NfoPath, nf);
            return s.ToFile();
        }
    }
}
