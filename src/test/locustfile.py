from locust import HttpUser, task, between

class QuickstartUser(HttpUser):
    wait_time = between(1, 2)

    @task
    def hello_world(self):
        self.client.get("/")
        self.client.get("/post/3/the-future-of-cms-from-mixcore")

    @task(3)
    def view_item(self):
        for item_id in range(1, 3, 1):
            self.client.get(f"/post/{item_id}/the-future-of-cms-from-mixcore")