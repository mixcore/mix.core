200 OK — Response was successful
201 Created — The entity submitted in the request body was created (synchronously)
202 Accepted — The entity submitted in the request body will be created (asynchronously)
204 No Content — No need to update the view
205 Reset Content — Client should reset the view
206 Partial Content —Partial content returned (e.g. ranged or paginated content)
400 Bad Request — The request was malformed
401 Unauthorized — The client is not authenticated with the server
403 Forbidden — The client is authenticated with the server, but not authorized to perform the requested operation on the requested resource
405 Method Not Allowed — The HTTP method used is not allowed on the requested URL
409 Conflict — There was a conflict when performing the operation, for example, the request attempted to update a resource that had already changed
500 Internal Server Error — An error on the server occurred and was not handled
501 Not Implemented —The HTTP method is not currently implemented for the requested resource
503 Service Unavailable — The server or one of it’s dependencies (such as a database) is unable to respond due to overload, outages, etc.