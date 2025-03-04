document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById('matrixCanvas');
    const ctx = canvas.getContext('2d');

    let columns;
    let drops;

    function resizeCanvas() {
        canvas.width = document.documentElement.scrollWidth; 
        canvas.height = document.documentElement.scrollHeight; 

        columns = Math.floor(canvas.width / fontSize);
        drops = [];
        for (let x = 0; x < columns; x++) {
            drops[x] = Math.floor(Math.random() * canvas.height / fontSize); 
        }
    }

    const matrix = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789@#$%^&*()*&^%";
    const fontSize = 20;

    resizeCanvas();
    window.addEventListener('resize', resizeCanvas);

    function draw() {
        ctx.fillStyle = "rgba(0, 0, 0, 0.05)";
        ctx.fillRect(0, 0, canvas.width, canvas.height);

        ctx.fillStyle = "#00008B"; 
        ctx.font = fontSize + "px arial";

        for (let i = 0; i < drops.length; i++) {
            const text = matrix[Math.floor(Math.random() * matrix.length)];
            ctx.fillText(text, i * fontSize, drops[i] * fontSize);

            if (drops[i] * fontSize > canvas.height && Math.random() > 0.975) {
                drops[i] = 0;
            }

            drops[i] += 0.5; 
        }
    }

    setInterval(draw, 50); 
});

