# Тестовое задание Consimple

Тут хранится код интернет-магазина, тестового задания компании Consimple.

Реализованы требуемые функции:
- Возвращает список клиентов у которых сегодня день рождение.
- Последние покупатели.
- Возвращает список категорий продуктов, которые покупал клиент. Для каждой категории возвращает количество купленных единиц.

*Примечание.*

На основе данного приложения уже можно построить интернет-магазин, но только если оперировать сущностями через CRUD методы апи. Также сознательно не было использовано DTO для моделей и трехслойной архитектуры (логика написана сразу в контроллерах) для скорейшей реализации. Упор был сделан только на требуемые в тестовом методы апи.

Для тестирования был написан метод сидирования базы данных (заполняет категориями, товарами, пользователями, запросами и покупками):
`GET https://localhost:5001/api/v1/common/seedDatabase`

## Методы апи из тестового задания

1. Для именинников:

`GET https://localhost:5001/api/v1/users/getByBirthday?month=1&day=10`

2. Для получения последних покупателей:

`GET https://localhost:5001/api/v1/users/getLastByers?days=10`

Возвращают такой json:

```json
[
	{
		"id": 1,
		"full_name": "Саня",
		"birth_date": "1999-01-10T00:00:00",
		"registration_date": "2020-11-14T00:00:00",
		"purchases": [
			{
				"id": 24,
				"date": "2021-02-08T06:30:38.0223561",
				"total_cost": 19999,
				"user_id": 1,
				"purchase_positions": [
					{
						"id": 45,
						"purchase_id": 24,
						"product_id": 2,
						"product": {
							"id": 2,
							"title": "Какой-то комп",
							"vendor_code": "Арт.581148",
							"price": 19999,
							"product_category_id": 3,
							"product_category": {
								"id": 3,
								"category_name": "Електроника",
								"products": [
									{
										"id": 3,
										"title": "Какой-то холодильник",
										"vendor_code": "Арт.654812",
										"price": 14999,
										"product_category_id": 3
									},
									{
										"id": 1,
										"title": "Какой-то ноутбук",
										"vendor_code": "Арт.121663",
										"price": 23999,
										"product_category_id": 3
									}
								]
							}
						},
						"products_multiplier": 1
					}
				]
			}
		]
	}
]
```

При этом в бд все максимально компактно.

3. Статистика покупок:

`GET https://localhost:5001/api/v1/users/getPurchasesStats?id=1`

Возвращает:

```json
[
  {
    "category_id": 3,
    "category_name": "Електроника",
    "buy_count": 4
  },
  {
    "category_id": 6,
    "category_name": "Ремонт",
    "buy_count": 3
  },
  {
    "category_id": 5,
    "category_name": "Для огорода",
    "buy_count": 4
  },
  {
    "category_id": 4,
    "category_name": "Мебель",
    "buy_count": 1
  }
]
```

## База данных

В качестве СУБД использовался Microsoft SQL Server. Схема базы данных сгенерированна по моделям (Code-First) при помощи Entity Framework.

Команды для обновления схемы БД:

`dotnet ef migrations add AddMigration_* --project ConsimpleTestTask.WebApp`

`dotnet ef database update --project ConsimpleTestTask.WebApp`




## Деплой 

Можно задеплоить на Heroku нажав кнопку.

[![Deploy](https://www.herokucdn.com/deploy/button.svg)](https://heroku.com/deploy?template=https://github.com/ExtremeDotneting/ConsimpleTestTask)

