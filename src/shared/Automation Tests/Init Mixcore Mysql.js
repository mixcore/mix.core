const puppeteer = require('puppeteer'); // v13.0.0 or later

(async () => {
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    const timeout = 15000;
    page.setDefaultTimeout(timeout);

    {
        const targetPage = page;
        await targetPage.setViewport({"width":976,"height":755})
    }
    {
        const targetPage = page;
        const promises = [];
        promises.push(targetPage.waitForNavigation());
        await targetPage.goto("https://localhost:5010/init");
        await Promise.all(promises);
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/My Best Site Ever! - Mixcore CMS"],["body > div.page.page-center.d-block > div > div > div.card.card-md > div:nth-child(3) > form > div > div:nth-child(1) > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        await element.click({
          offset: {
            x: 113,
            y: 13.012496948242188,
          },
        });
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/My Best Site Ever! - Mixcore CMS"],["body > div.page.page-center.d-block > div > div > div.card.card-md > div:nth-child(3) > form > div > div:nth-child(1) > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("test mysql");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("test mysql");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "test mysql");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Tab");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/Domain, IP or Docker service IP"],["body > div.page.page-center.d-block > div > div > div.card.card-md > div:nth-child(3) > form > div > mysql-info > div:nth-child(1) > div.mb-3.col-8 > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        await element.click({
          offset: {
            x: 112,
            y: 21.012481689453125,
          },
        });
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/Domain, IP or Docker service IP"],["body > div.page.page-center.d-block > div > div > div.card.card-md > div:nth-child(3) > form > div > mysql-info > div:nth-child(1) > div.mb-3.col-8 > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("localhost");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("localhost");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "localhost");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Tab");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/mixcore"],["body > div.page.page-center.d-block > div > div > div.card.card-md > div:nth-child(3) > form > div > mysql-info > div.mb-3 > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("mixc0re");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("mixc0re");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "mixc0re");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Tab");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/root"],["body > div.page.page-center.d-block > div > div > div.card.card-md > div:nth-child(3) > form > div > mysql-info > div:nth-child(3) > div:nth-child(1) > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("root");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("root");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "root");
        }
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/Continue"],["body > div.page.page-center.d-block > div > div > div.row.align-items-center.mt-3 > div.col > div > button"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        await element.click({
          offset: {
            x: 12.7249755859375,
            y: 21.61248779296875,
          },
        });
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/administrator"],["body > div.page.page-center.d-block > div > div > form > div.card.card-md > div:nth-child(3) > div > div:nth-child(1) > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        await element.click({
          offset: {
            x: 130,
            y: 18.600006103515625,
          },
        });
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/administrator"],["body > div.page.page-center.d-block > div > div > form > div.card.card-md > div:nth-child(3) > div > div:nth-child(1) > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("admin");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("admin");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "admin");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Shift");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["body > div.page.page-center.d-block > div > div > form > div.card.card-md > div:nth-child(3) > div > div.row > div:nth-child(1) > div > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("P@");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("P@");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "P@");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Shift");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["body > div.page.page-center.d-block > div > div > form > div.card.card-md > div:nth-child(3) > div > div.row > div:nth-child(1) > div > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("P@ssw0rd");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("P@ssw0rd");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "P@ssw0rd");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Shift");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["body > div.page.page-center.d-block > div > div > form > div.card.card-md > div:nth-child(3) > div > div.row > div:nth-child(2) > div > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("P@");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("P@");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "P@");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Shift");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["body > div.page.page-center.d-block > div > div > form > div.card.card-md > div:nth-child(3) > div > div.row > div:nth-child(2) > div > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("P@ssw0rd");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("P@ssw0rd");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "P@ssw0rd");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Tab");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/your-email-address@domain.com"],["body > div.page.page-center.d-block > div > div > form > div.card.card-md > div:nth-child(3) > div > div:nth-child(3) > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("asd");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("asd");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "asd");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Shift");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/your-email-address@domain.com"],["body > div.page.page-center.d-block > div > div > form > div.card.card-md > div:nth-child(3) > div > div:nth-child(3) > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("asd@");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("asd@");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "asd@");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Shift");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/your-email-address@domain.com"],["body > div.page.page-center.d-block > div > div > form > div.card.card-md > div:nth-child(3) > div > div:nth-child(3) > input"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        const type = await element.evaluate(el => el.type);
        if (["select-one"].includes(type)) {
          await element.select("asd@asdfsa");
        } else if (["textarea","text","url","tel","search","password","number","email"].includes(type)) {
          await element.type("asd@asdfsa");
        } else {
          await element.focus();
          await element.evaluate((el, value) => {
            el.value = value;
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
          }, "asd@asdfsa");
        }
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up("Tab");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.down(" ");
    }
    {
        const targetPage = page;
        await targetPage.keyboard.up(" ");
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/Continue"],["body > div.page.page-center.d-block > div > div > form > div.row.align-items-center.mt-3 > div.col > div > button"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        await element.click({
          offset: {
            x: 79.7249755859375,
            y: 14.5999755859375,
          },
        });
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["#frm-theme > div.card.card-md > div > div > div > div.mb-3 > ul > li:nth-child(2) > a > span.ms-2.ng-binding.ng-scope"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        await element.click({
          offset: {
            x: 41.787506103515625,
            y: 9,
          },
        });
    }
    {
        const targetPage = page;
        const element = await waitForSelectors([["aria/Continue"],["#frm-theme > div.row.align-items-center.mt-3 > div.col > div > button"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        await element.click({
          offset: {
            x: 112,
            y: 21.8125,
          },
        });
    }
    {
        const targetPage = page;
        const promises = [];
        promises.push(targetPage.waitForNavigation());
        const element = await waitForSelectors([["aria/Continue"],["#frm-theme > div.row.align-items-center.mt-3 > div.col > div > button"]], targetPage, { timeout, visible: true });
        await scrollIntoViewIfNeeded(element, timeout);
        await element.click({
          offset: {
            x: 88,
            y: 20.79998779296875,
          },
        });
        await Promise.all(promises);
    }

    await browser.close();

    async function waitForSelectors(selectors, frame, options) {
      for (const selector of selectors) {
        try {
          return await waitForSelector(selector, frame, options);
        } catch (err) {
          console.error(err);
        }
      }
      throw new Error('Could not find element for selectors: ' + JSON.stringify(selectors));
    }

    async function scrollIntoViewIfNeeded(element, timeout) {
      await waitForConnected(element, timeout);
      const isInViewport = await element.isIntersectingViewport({threshold: 0});
      if (isInViewport) {
        return;
      }
      await element.evaluate(element => {
        element.scrollIntoView({
          block: 'center',
          inline: 'center',
          behavior: 'auto',
        });
      });
      await waitForInViewport(element, timeout);
    }

    async function waitForConnected(element, timeout) {
      await waitForFunction(async () => {
        return await element.getProperty('isConnected');
      }, timeout);
    }

    async function waitForInViewport(element, timeout) {
      await waitForFunction(async () => {
        return await element.isIntersectingViewport({threshold: 0});
      }, timeout);
    }

    async function waitForSelector(selector, frame, options) {
      if (!Array.isArray(selector)) {
        selector = [selector];
      }
      if (!selector.length) {
        throw new Error('Empty selector provided to waitForSelector');
      }
      let element = null;
      for (let i = 0; i < selector.length; i++) {
        const part = selector[i];
        if (element) {
          element = await element.waitForSelector(part, options);
        } else {
          element = await frame.waitForSelector(part, options);
        }
        if (!element) {
          throw new Error('Could not find element: ' + selector.join('>>'));
        }
        if (i < selector.length - 1) {
          element = (await element.evaluateHandle(el => el.shadowRoot ? el.shadowRoot : el)).asElement();
        }
      }
      if (!element) {
        throw new Error('Could not find element: ' + selector.join('|'));
      }
      return element;
    }

    async function waitForElement(step, frame, timeout) {
      const count = step.count || 1;
      const operator = step.operator || '>=';
      const comp = {
        '==': (a, b) => a === b,
        '>=': (a, b) => a >= b,
        '<=': (a, b) => a <= b,
      };
      const compFn = comp[operator];
      await waitForFunction(async () => {
        const elements = await querySelectorsAll(step.selectors, frame);
        return compFn(elements.length, count);
      }, timeout);
    }

    async function querySelectorsAll(selectors, frame) {
      for (const selector of selectors) {
        const result = await querySelectorAll(selector, frame);
        if (result.length) {
          return result;
        }
      }
      return [];
    }

    async function querySelectorAll(selector, frame) {
      if (!Array.isArray(selector)) {
        selector = [selector];
      }
      if (!selector.length) {
        throw new Error('Empty selector provided to querySelectorAll');
      }
      let elements = [];
      for (let i = 0; i < selector.length; i++) {
        const part = selector[i];
        if (i === 0) {
          elements = await frame.$$(part);
        } else {
          const tmpElements = elements;
          elements = [];
          for (const el of tmpElements) {
            elements.push(...(await el.$$(part)));
          }
        }
        if (elements.length === 0) {
          return [];
        }
        if (i < selector.length - 1) {
          const tmpElements = [];
          for (const el of elements) {
            const newEl = (await el.evaluateHandle(el => el.shadowRoot ? el.shadowRoot : el)).asElement();
            if (newEl) {
              tmpElements.push(newEl);
            }
          }
          elements = tmpElements;
        }
      }
      return elements;
    }

    async function waitForFunction(fn, timeout) {
      let isActive = true;
      setTimeout(() => {
        isActive = false;
      }, timeout);
      while (isActive) {
        const result = await fn();
        if (result) {
          return;
        }
        await new Promise(resolve => setTimeout(resolve, 100));
      }
      throw new Error('Timed out');
    }
})();
