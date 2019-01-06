modules.component('snowy', {
    templateUrl: '/app/app-shared/components/snowy/index.html',
    controller: ['$scope', function ($scope) {
        var ctrl = this;
        this.init = function () {
            var Snowflake = (function() {

                var flakes;
                var flakesTotal = 250;
                var wind = 0;
                var mouseX;
                var mouseY;
            
                function Snowflake(size, x, y, vx, vy) {
                    this.size = size;
                    this.x = x;
                    this.y = y;
                    this.vx = vx;
                    this.vy = vy;
                    this.hit = false;
                    this.melt = false;
                    this.div = document.createElement('div');
                    this.div.classList.add('snowflake');
                    this.div.style.width = this.size + 'px';
                    this.div.style.height = this.size + 'px';
                }
            
                Snowflake.prototype.move = function() {
                    if (this.hit) {
                        if (Math.random() > 0.995) this.melt = true;
                    } else {
                        this.x += this.vx + Math.min(Math.max(wind, -10), 10);
                        this.y += this.vy;
                    }
            
                    // Wrap the snowflake to within the bounds of the page
                    if (this.x > window.innerWidth + this.size) {
                        this.x -= window.innerWidth + this.size;
                    }
            
                    if (this.x < -this.size) {
                        this.x += window.innerWidth + this.size;
                    }
            
                    if (this.y > window.innerHeight + this.size) {
                        this.x = Math.random() * window.innerWidth;
                        this.y -= window.innerHeight + this.size * 2;
                        this.melt = false;
                    }
            
                    var dx = mouseX - this.x;
                    var dy = mouseY - this.y;
                    this.hit = !this.melt && this.y < mouseY && dx * dx + dy * dy < 2400;
                };
            
                Snowflake.prototype.draw = function() {
                    this.div.style.transform =
                    this.div.style.MozTransform =
                    this.div.style.webkitTransform =
                        'translate3d(' + this.x + 'px' + ',' + this.y + 'px,0)';
                };
            
                function update() {
                    for (var i = flakes.length; i--; ) {
                        var flake = flakes[i];
                        flake.move();
                        flake.draw();
                    }
                    requestAnimationFrame(update);
                }
            
                Snowflake.init = function(container) {
                    flakes = [];
            
                    for (var i = flakesTotal; i--; ) {
                        var size = (Math.random() + 0.2) * 2 + 1;
                        var flake = new Snowflake(
                            size,
                            Math.random() * window.innerWidth,
                            Math.random() * window.innerHeight,
                            Math.random() - 0.5,
                            size * 0.3
                        );
                        container.appendChild(flake.div);
                        flakes.push(flake);
                    }
                
                container.onmousemove = function(event) {
                      mouseX = event.clientX;
                      mouseY = event.clientY;
                      wind = (mouseX - window.innerWidth / 2) / window.innerWidth * 6;
                  };
            
                  container.ontouchstart = function(event) {
                      mouseX = event.targetTouches[0].clientX;
                      mouseY = event.targetTouches[0].clientY;
                      event.preventDefault();
                  };
            
                  window.ondeviceorientation = function(event) {
                      if (event) {
                          wind = event.gamma / 10;
                      }
                  };
                
                  update();
                };
            
                return Snowflake;
            
            }());
            
            window.onload = function() {
              setTimeout(function() {
                  Snowflake.init(document.getElementById('snow'));
              }, 500);
            }

        };
    }],
    bindings: {
    }
});