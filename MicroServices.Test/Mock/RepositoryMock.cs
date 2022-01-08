using System;
using System.Collections.Generic;
using System.Linq;
using MicroServices.Core.Entity;
using MicroServices.Infrastructure.Repository;
using Moq;

namespace MicroServices.Test.Mock
{
    public class RepositoryMock
    {
        public void SetupMock<TDocument>(Moq.Mock mockRepo, List<TDocument> collection) where TDocument : DocumentEntity
        {
            var mock = mockRepo.As<IRepository<TDocument>>();

            mock.Setup(x => x.Get(null)).Returns(collection.AsQueryable());
            
            mock.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Func<string, TDocument>(
                    x => collection.Find(q => q.Id.Equals(x))
                ));

            
            mock.Setup(x => x.CreateAsync(It.IsAny<TDocument>()))
                .ReturnsAsync(new Func<TDocument, TDocument>(x =>
                {
                    x.Id = new Guid().ToString();
                    collection.Add(x);
                    return collection.Last();
                }));
            
            mock.Setup(x => x.UpdateAsync(It.IsAny<TDocument>()))
                .ReturnsAsync(new Func<TDocument, TDocument>(x =>
                {
                    var i = collection.FindIndex(q => q.Id.Equals(x.Id));
                    collection[i] = x;
                    return collection[i];
                }));
            
            mock.Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(new Func<string, TDocument>(x =>
                {
                    var deleted=  collection.FirstOrDefault(c => c.Id == x);
                    collection.RemoveAll(q => q.Id == x);
                    return deleted;
                }));
        }
    }
}