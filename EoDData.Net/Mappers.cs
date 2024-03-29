﻿using AutoMapper;
using Cph.FinancialDataSanitizer;

namespace EoDData.Net
{
    public class Mappers : Profile
    {
        private readonly AbbreviationParser _parser;

        public Mappers()
        {
            ConfigureMappings();
            _parser = new AbbreviationParser();
        }

        public void ConfigureMappings()
        {
            CreateMap<Symbol, Symbol>()
                .ForMember(dest =>
                    dest.Name, map =>
                    map.MapFrom(src => _parser.ExpandAllAbbreviationsFromString(src.Name, true)));
        }
    }
}
