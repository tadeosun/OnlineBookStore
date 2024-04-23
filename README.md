# EBookStore

Online Book Store API Documentation
Welcome to the Online Book Store API documentation. This API allows you to manage books, users, and shopping carts for an online bookstore. It provides endpoints for performing various operations such as adding, retrieving, and updating books, managing users, and handling shopping cart functionality and payment.

Base URL
The base URL for accessing the API is https://localhost:7060/ .

Authentication
Authentication is required to access certain endpoints. The API uses JSON Web Tokens (JWT) for authentication. To authenticate, include a valid JWT token bearer in the Authorization header of your HTTP requests.

Endpoints

A. Inventory
1. Get All Inventory
•	URL: /Inventory/api/GetAllInventory
•	Method: GET
•	Description: Retrieve a list of all books in stock.
•	Authentication: Not required
2. Search Inventory 
•	URL: /Inventory/api/SearchInventory?searchTerm= searchTerm
•	Method: GET
•	Description: Retrieve a list of books based on criteria supplied.
•	Authentication: Not required
3. Retrieve Book by ID
•	URL: /Inventory/api/GetInventory?id=id
•	Method: GET
•	Description: Retrieve details of a specific book by its ID.
•	Authentication: Not required
4. Add New Inventory
•	URL: / Inventory/api/CreateInventory
•	Method: POST
•	Description: Add a new book to the bookstore.
•	Authentication: Required
•	Request Body: JSON object representing the book details
5. Update Inventory
•	URL: / Inventory/api/UpdateInventory
•	Method: POST
•	Description: Update details of a specific book.
•	Authentication: Required
•	Request Body: JSON object representing the updated book details

B. Login
Login
•	URL: /login
•	Method: POST
•	Description: Login to an existing user account.
•	Authentication: Not required
•	Request Body: JSON object representing the login credentials (username, email, password)

C. Users
1. Register User
•	URL: / User/api/AddUser
•	Method: POST
•	Description: Register a new user account.
•	Authentication: Not required
•	Request Body: JSON object representing the user details (username, email, password)
2. Retrieve User Profile
•	URL: /User/api/GetUserById?id=id
•	Method: GET
•	Description: Retrieve the profile details of the authenticated user.
•	Authentication: Required
3. Update User Profile
•	URL: /User/api/UpdateUser
•	Method: POST
•	Description: Update the profile details of the authenticated user.
•	Authentication: Required
•	Request Body: JSON object representing the updated user profile details

D. Shopping Cart
1. Add Item to Cart
•	URL: /Cart/api/CreateCart
•	Method: POST
•	Description: Add a book to the shopping cart.
•	Authentication: Required
•	Request Body: JSON object representing the cart item details (book ID, quantity)
2. Retrieve Cart
•	URL: / Cart/api/ViewCart?id=id
•	Method: GET
•	Description: Retrieve the contents of the shopping cart for the authenticated user.
•	Authentication: Required
3. Update Cart Item
•	URL: / Cart/api/CreateCart
•	Method: POST
•	Description: Update the quantity of a specific item in the shopping cart.
•	Authentication: Required
•	Request Body: JSON object representing the updated cart item details (quantity)
4. Remove Cart Item
•	URL: / Cart/api/RemoveItem?id=id
•	Method: DELETE
•	Description: Remove a specific item from the shopping cart.
•	Authentication: Required

E. Checkout
1. CheckOut
•	URL: /Checkout/api/SubmitOrder
•	Method: POST
•	Description: Add a book to the shopping cart.
•	Authentication: Required
•	Request Body: JSON object representing the cart item details (book ID, quantity)
2. PurchaseHistory
•	URL: / Checkout/api/PurchaseHistory
•	Method: GET
•	Description: Retrieve the previously completed orders of the authenticated user.
•	Authentication: Required

Error Responses
The API returns appropriate HTTP status codes along with error messages in case of errors. Below are some common error responses:
•	400 Bad Request: Invalid request format or parameters.
•	401 Unauthorized: Authentication failed or credentials are missing or invalid.
•	403 Forbidden: Access to the resource is forbidden.
•	404 Not Found: The requested resource is not found.
•	500 Internal Server Error: An unexpected error occurred on the server.


Contact
For any questions, issues, or feedback regarding the API, please contact us at tadeosun291@gmail.com.
Thank you for using the Online Book Store API!

