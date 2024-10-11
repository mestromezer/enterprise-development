using Microsoft.AspNetCore.Mvc;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;
using Swashbuckle.AspNetCore.Annotations;

namespace Pharmacies.Controllers
{
    /// <summary>
    /// Контроллер для работы с сущностями, которые являются словарями
    /// </summary>
    [Route("api/[controller]/{dictionaryType}")]
    [ApiController]
    public class DictionariesController(
        IRepository<ProductGroup, int> productGroupRepository,
        IRepository<PharmaceuticalGroup, int> pharmaceuticalGroupRepository)
        : ControllerBase
    {
        /// <summary>
        /// Получить все записи словаря по его типу (product_group или pharmaceutical_group)
        /// </summary>
        /// <param name="dictionaryType">Тип словаря: product_group или pharmaceutical_group</param>
        [HttpGet("")]
        [SwaggerOperation(Summary = "Получить все записи словаря по типу", Description = "Доступные типы словарей: product_group, pharmaceutical_group.")]
        [SwaggerResponse(200, "Успешный ответ с данными словаря")]
        [SwaggerResponse(400, "Неправильный тип словаря")]
        public async Task<ActionResult<IEnumerable<object>>> GetDictionaryItems([FromRoute] string dictionaryType)
        {
            return dictionaryType.ToLower() switch
            {
                "product_group" => Ok(await productGroupRepository.GetAsList()),
                "pharmaceutical_group" => Ok(await pharmaceuticalGroupRepository.GetAsList()),
                _ => BadRequest("Invalid dictionary type.")
            };
        }

        /// <summary>
        /// Получить элемент словаря по типу и Id
        /// </summary>
        /// <param name="dictionaryType">Тип словаря: product_group или pharmaceutical_group</param>
        /// <param name="id">Идентификатор элемента</param>
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Получить элемент словаря по типу и Id", Description = "Доступные типы словарей: product_group, pharmaceutical_group.")]
        [SwaggerResponse(200, "Успешный ответ с данными элемента")]
        [SwaggerResponse(404, "Элемент не найден")]
        [SwaggerResponse(400, "Неправильный тип словаря")]
        public async Task<ActionResult<object>> GetDictionaryItem([FromRoute] string dictionaryType, [FromRoute] int id)
        {
            switch (dictionaryType.ToLower())
            {
                case "product_group":
                    var productGroup = (await productGroupRepository.GetAsList(pg => pg.Id == id)).FirstOrDefault();
                    if (productGroup == null) return NotFound();
                    return Ok(productGroup);

                case "pharmaceutical_group":
                    var pharmaceuticalGroup =
                        (await pharmaceuticalGroupRepository.GetAsList(pg => pg.Id == id)).FirstOrDefault();
                    if (pharmaceuticalGroup == null) return NotFound();
                    return Ok(pharmaceuticalGroup);

                default:
                    return BadRequest("Invalid dictionary type.");
            }
        }

        /// <summary>
        /// Добавить новый элемент в словарь
        /// </summary>
        /// <param name="dictionaryType">Тип словаря: product_group или pharmaceutical_group</param>
        /// <param name="newItem">Новый элемент для добавления</param>
        [HttpPost("")]
        [SwaggerOperation(Summary = "Добавить элемент в словарь", Description = "Доступные типы словарей: product_group, pharmaceutical_group.")]
        [SwaggerResponse(201, "Элемент успешно создан")]
        [SwaggerResponse(400, "Неправильный тип словаря или элемент")]
        public async Task<ActionResult<object>> CreateDictionaryItem([FromRoute] string dictionaryType, [FromBody] string newItem)
        {
            switch (dictionaryType.ToLower())
            {
                case "product_group":
                    var productGroup = new ProductGroup() { Id = -1, Name = newItem };
                    await productGroupRepository.Add(productGroup);
                    return Ok();

                case "pharmaceutical_group":
                    var pharmaceuticalGroup = new PharmaceuticalGroup() { Id = -1, Name = newItem };
                    await pharmaceuticalGroupRepository.Add(pharmaceuticalGroup);
                    return Ok();
            }

            return BadRequest("Invalid dictionary type or item.");
        }

        /// <summary>
        /// Обновить элемент словаря
        /// </summary>
        /// <param name="dictionaryType">Тип словаря: product_group или pharmaceutical_group</param>
        /// <param name="id">Идентификатор элемента</param>
        /// <param name="updatedItem">Обновленный элемент</param>
        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Обновить элемент словаря", Description = "Доступные типы словарей: product_group, pharmaceutical_group.")]
        [SwaggerResponse(204, "Элемент успешно обновлен")]
        [SwaggerResponse(400, "Неправильный тип словаря или элемент")]
        public async Task<IActionResult> UpdateDictionaryItem([FromRoute] string dictionaryType, [FromRoute] int id, [FromBody] string updatedItem)
        {
            switch (dictionaryType.ToLower())
            {
                case "product_group":
                    var productGroup = new ProductGroup() { Id = id, Name = updatedItem };
                    await productGroupRepository.Add(productGroup);
                    return Ok();

                case "pharmaceutical_group":
                    var pharmaceuticalGroup = new PharmaceuticalGroup() { Id = id, Name = updatedItem };
                    await pharmaceuticalGroupRepository.Add(pharmaceuticalGroup);
                    return Ok();
            }

            return BadRequest("Invalid dictionary type or item.");
        }

        /// <summary>
        /// Удалить элемент словаря по Id
        /// </summary>
        /// <param name="dictionaryType">Тип словаря: product_group или pharmaceutical_group</param>
        /// <param name="id">Идентификатор элемента</param>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Удалить элемент словаря по Id", Description = "Доступные типы словарей: product_group, pharmaceutical_group.")]
        [SwaggerResponse(204, "Элемент успешно удален")]
        [SwaggerResponse(400, "Неправильный тип словаря")]
        public async Task<IActionResult> DeleteDictionaryItem([FromRoute] string dictionaryType, [FromRoute] int id)
        {
            switch (dictionaryType.ToLower())
            {
                case "product_group":
                    await productGroupRepository.Delete(id);
                    return NoContent();

                case "pharmaceutical_group":
                    await pharmaceuticalGroupRepository.Delete(id);
                    return NoContent();

                default:
                    return BadRequest("Invalid dictionary type.");
            }
        }
    }
}
