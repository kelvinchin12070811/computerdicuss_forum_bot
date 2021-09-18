import express, { application } from 'express';

const app = express();
const port = 3000;

app.get('/', (req, res) => {
    res.send('<h1>Hello World!</h1>');
});

app.listen(port, () => console.log(`Server is started at port ${port}.`));