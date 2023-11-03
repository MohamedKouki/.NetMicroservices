using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
[Route("Items")]
    public class ItemController : ControllerBase
    {
       public readonly IRepository<Item> itemsRepository;

       public ItemController(IRepository<Item> itemsRepository)
       {
        this.itemsRepository=itemsRepository;
       }
        
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {   
            var items = (await itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item=await itemsRepository.GetAsync(id);

            if(item == null){
                return NotFound();
            }

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {

            var item =new Item
            {
                Name =createItemDto.Name,
                Description =createItemDto.Description,
                Price=createItemDto.Price,
                CreateDate=DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new {id = item.Id} , item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {   
            var existingitem = await itemsRepository.GetAsync(id);

            if(existingitem == null)
            {
                return NotFound();
            }
           
           existingitem.Name = updateItemDto.Name;
           existingitem.Description = updateItemDto.Description;
           existingitem.Price = updateItemDto.Price;

           await itemsRepository.UpdateAsync(existingitem);
           return NoContent();
        }
    [HttpDelete("{id}")]
    public async Task<ActionResult<ItemDto>> DeleteAsync(Guid id){

        var item =await itemsRepository.GetAsync(id);

        if(item == null)
        {
            return NotFound();
        }

        await itemsRepository.RemoveAsync(item.Id);

        return NoContent();

    }
        
}

    public class InterfaceItemRepository
    {
    }
}


