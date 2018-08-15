import 'font-awesome/css/font-awesome.css';
import 'bootstrap/dist/css/bootstrap.css';

import './main.css';
import { Main } from './Main.elm';
import registerServiceWorker from './registerServiceWorker';

Main.embed(document.getElementById('root'));

registerServiceWorker();
