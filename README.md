# Project Title: Revised Online Payment Transaction System

The **Revised Online Payment Transaction System** is a C# application designed to streamline the payment processing workflow within the online payment department. It facilitates the handling of various tax payments, including real property tax, business application tax, and miscellaneous taxes, by providing a comprehensive platform for monitoring payment statuses and assisting departmental staff in the tax payment process.

It's important to note that the system does not directly handle tax payments; rather, it supports departmental staff in their administrative tasks. Here's an overview of the different stages involved in tax processing:

- **Verification:** This stage involves confirming the entry of payments into the system. Staff input payment data from various sources, such as banks, GCash, and Paymaya, into the system for further processing.

- **Validation:** Once payments are verified, staff update the system to reflect the receipt of payments by the department. Verifiers cross-check payments received from other systems and update the system accordingly.

- **Encoding:** This segment of the system is dedicated to encoding tax payments received from different sources. Payments from banks and electronic platforms like GCash and Paymaya are entered into the system by departmental staff.

- **Official Receipt Issuance:** Tax collectors utilize this portion of the system to generate official receipts upon successful validation of payments. The system enables them to tag generated official receipts, indicating completion of the validation process.

- **Official Receipt Upload:** Receipts are scanned, securely stored in the database, and automatically sent to taxpayers via email through this feature. Additionally, staff update information regarding the physical storage location of receipts, facilitating easy retrieval when needed.

- **Releasing:** This module assists staff when taxpayers claim their physical receipts. The system aids in locating the group of taxes associated with a particular taxpayer and provides information on the cabinet and folder containing the physical copies for retrieval.

By efficiently managing the tax payment workflow, the **Revised Online Payment Transaction System** enhances the productivity of the online payment department and ensures smoother processing of tax payments.

## Technical Information

The codebase adheres to a structured convention, where classes are organized according to specific roles, ensuring each class maintains a singular responsibility. This design principle enhances code clarity and maintainability.

Key technologies utilized in this project include:

- **C#:** The primary programming language employed for development.
- **.NET 6.0:** Leveraged for its latest features and enhancements, providing a robust framework for building modern applications.
- **Winforms:** Utilized for user interface (UI) development and orchestration of system workflows.
- **Entity Framework (EF):** Employs EF as the primary Object-Relational Mapping (ORM) tool, facilitating seamless interaction with the MS SQL Server database.
- **Entity Framework LINQ:** Leveraged extensively for database queries and interactions, enhancing code readability and efficiency.
- **Raw SQL:** Employed for executing complex database queries, offering flexibility and control over data retrieval and manipulation.
- **Layered Architecture:** Adopts a layered architectural approach, segregating components into distinct layers:
  - **Winforms:** Responsible solely for UI presentation and user interaction.
  - **Services:** Implements business logic and transaction management, ensuring cohesive application behavior.
  - **DAL (Data Access Layer):** Handles database interactions, providing a separation of concerns and promoting maintainability.
- **OpenCVSharp:** Utilized for generating Excel reports, enabling efficient data visualization and analysis.

By leveraging these technologies and adhering to sound architectural principles, the system delivers robust functionality and maintainability.
