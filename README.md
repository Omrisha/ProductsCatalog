# ProductsCatalog

This project is a simple API for managing products and catalogs.

## Prerequisites
- .NET 6.0 SDK
- Docker
- Visual Studio/Rider
- Postman (optional)
- MongoDB (optional)

## Installation

### Option 1: IDE (Visual Studio/Rider)
- Run MongoDB on your local machine, or use docker command 
- `docker container run -p 27017:27017 --name mongodbcontainer -e MONGO_INITDB_ROOT_USERNAME="root" -e MONGO_INITDB_ROOT_PASSWORD="rootpassword" -v mongodb_data_container:/data/db mongo:latest`
- Clone the repository `git clone https://github.com/Omrisha/ProductsCatalog.git`
- Open the solution in Visual Studio/Rider or any IDE of your choice.
- Run the project.
- The API will be available at `http://localhost:8006/docs`

### Option 2: Docker
- Clone the repository `git clone https://github.com/Omrisha/ProductsCatalog.git`
- Run `docker-compose up` in the root directory of the project.
- The API will be available at `http://localhost:8006/docs`
- Run `docker-compose down` to stop the containers.


## Endpoints
### Products endpoints:
- Create a new product
- Get all products
- Get a product by Id
- Get products by Category - products that match the category.
- Get products by Price limit - get products with a lower price
- Update a product
- Delete a product
### Catalogs endpoints:
- Get all Catalogs
- Get a Catalog by Id
- Get a CatalogId by ProductId - returns the catalog Id/s which contains the product.
- Update a catalog


## Product CRUD - JSON Examples

### POST /Products 
- Body:
```json
{
  "title": "Groot",
  "price": 100,
  "category": "FreshProduct",
  "description": "An electric groot robot from Guardians of the Galaxy",
  "uniqueProperties": [
    {
      "name": "ExpiryDate",
      "value": "2025-09-11"
    }
  ]
}
```

### GET /Products
```json

  {
    "id": "2f5eee20-a0ca-4a42-9ead-a57840789f53",
    "title": "Groot",
    "description": "An electric groot robot from Guardians of the Galaxy",
    "price": 100,
    "category": "FreshProduct",
    "isActive": false,
    "uniqueProperties": [
      {
        "name": "ExpiryDate",
        "value": "2025-09-11"
      }
    ]
  },
  {
    "id": "954f880b-1391-46be-9113-7741c7fa76fc",
    "title": "iPhone 15 Pro",
    "description": "A celular phone by Apple Inc",
    "price": 1000,
    "category": "ElectricProduct",
    "isActive": false,
    "uniqueProperties": [
      {
        "name": "SocketType",
        "value": "US"
      },
      {
        "name": "Voltage",
        "value": "110v"
      }
    ]
  }
]
```

## Catalog CRUD

### POST /Catalogs
- Body:
```json
{
  "title": "Guardians Of The Galaxy Catalog",
  "products": [
    "2f5eee20-a0ca-4a42-9ead-a57840789f53"
  ]
}
```

### GET /Catalogs/{id}
```json
{
  "id": "eeb0c0f7-7e87-47ab-ac0e-3c7cec7e501c",
  "title": "Guardians Of The Galaxy Catalog 2",
  "products": [
    {
      "id": "2f5eee20-a0ca-4a42-9ead-a57840789f53",
      "title": "Groot",
      "description": "An electric groot robot from Guardians of the Galaxy",
      "price": 100,
      "category": "FreshProduct",
      "isActive": false,
      "uniqueProperties": [
        {
          "name": "ExpiryDate",
          "value": "2025-09-11"
        }
      ]
    },
    {
      "id": "954f880b-1391-46be-9113-7741c7fa76fc",
      "title": "iPhone 15 Pro",
      "description": "A celular phone by Apple Inc",
      "price": 1000,
      "category": "ElectricProduct",
      "isActive": false,
      "uniqueProperties": [
        {
          "name": "SocketType",
          "value": "US"
        },
        {
          "name": "Voltage",
          "value": "110v"
        }
      ]
    }
  ]
}
```
