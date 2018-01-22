using Sprotify.Domain.Services;
using System;
using System.Collections.Generic;
using Sprotify.Domain.Models;
using System.Threading.Tasks;
using Sprotify.Domain.Repositories;
using Sprotify.DAL;

namespace Sprotify.Application.Services
{
    public class BandService : IBandService
    {
        private readonly IBandRepository _bandRepository;
        private readonly UnitOfWork _unitOfWork;

        public BandService(
            IBandRepository bandRepository,
            UnitOfWork unitOfWork
        )
        {
            _bandRepository = bandRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<Band> GetBandById(Guid id)
        {
            return _bandRepository.GetBandById(id);
        }

        public Task<IEnumerable<Band>> GetBands(string filter)
        {
            return _bandRepository.GetBands(filter);
        }

        public async Task<Band> CreateBand(string name)
        {
            var band = new Band(name);
            _bandRepository.CreateBand(band);
            await _unitOfWork.SaveChanges().ConfigureAwait(false);

            return band;
        }

        public async Task<Band> UpdateBand(Band band)
        {
            await _unitOfWork.SaveChanges().ConfigureAwait(false);

            return band;
        }
    }
}
