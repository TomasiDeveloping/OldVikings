import {Component, inject, OnInit, Renderer2} from '@angular/core';

@Component({
  selector: 'app-s4-calculator',
  templateUrl: './s4-calculator.component.html',
  styleUrl: './s4-calculator.component.scss'
})
export class S4CalculatorComponent implements OnInit {

  private readonly _renderer: Renderer2 = inject(Renderer2);

  ngOnInit() {
    this.loadCalconicScript();
  }

  loadCalconicScript(): void {
    const scriptId = 'calconic-script';
    if (!document.getElementById(scriptId)) {
      const script = this._renderer.createElement('script');
      script.id = scriptId;
      script.type = 'text/javascript';
      script.async = true;
      script.src = 'https://cdn.calconic.com/static/js/calconic.min.js';
      script.setAttribute('data-calconic', 'true');
      this._renderer.appendChild(document.body, script);
    }
  }

}
