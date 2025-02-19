namespace PetFamily.Domain.Models.Pet;

public class Pet
{
    private Pet()
    {
    }

    public Pet()
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Gender { get; private set; }

    public string Description { get; private set; }

    // Порода
    public string Color { get; private set; }

    // Информация о здоровье питомца
    // Адрес, где находится питомец
    public int Weight { get; private set; }
    public int Height { get; private set; }

    public string PhoneNumber { get; private set; }

    // Кастрирован или нет
    public string Birthday { get; private set; }

    // Вакцинирован или нет
    // Статус помощи - Нуждается в помощи/Ищет дом/Нашел дом
    // Реквизиты для помощи (у каждого реквизита будет название и описание, как сделать перевод), поэтому нужно сделать отдельный класс для реквизита. Это должен быть Value Object.
    public string CreatedAt { get; private set; }
    // У Pet добавить VO, который будем иметь ссылку на SpeciesId и BreedId
    
    <Result> Create // имплисид
}