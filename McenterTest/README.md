# Mcenter Application

## Models
This folder contains classes that define the structure of data models used across the application.
- JSON request and response models: Defines the structure of incoming and outgoing JSON data.
- Data models: Represents the objects and their properties that interact with the application logic.
- Entity classes: Defines the shape and validation rules for various entities used in the app.

## Services
This folder contains classes that handle business logic and communication with external services.
- API Authorization: Manages authentication and authorization logic for API requests.
- HttpClient creation: Handles the setup and configuration of HttpClient instances used for HTTP requests.
- HttpRequest management: Contains classes that manage sending and receiving data from external APIs.
- Service layer: Acts as an intermediary between the UI and the data models, handling all data manipulation.

## UI
This folder contains the user interface components based on the MVVM (Model-View-ViewModel) pattern.
- Views: XAML files or view classes that define the visual aspects of the application.
- ViewModels: Classes that contain the logic binding data between the views and models.
- Controls: Custom user controls that enhance the functionality of the UI.
- Bindings: Contains data binding logic for the application, connecting views to the underlying data.

## Resources
This folder holds the non-code assets used by the application, like images and other visual elements.
- Images: Icons, backgrounds, and other graphical elements used within the UI.
- Fonts: Custom fonts used for application styling.
- Localization files: Resource files used for supporting multiple languages (e.g., .resx files).
- Styles: Contains styles and resource dictionaries used to maintain consistent visuals across the app.

## Theme
This folder contains themed UI components and resources that define the look and feel of the application.
- Color schemes: Definitions of color palettes used throughout the application.
- Themed controls: Customized versions of standard UI components that fit the application’s theme.
- Theme settings: Configuration files or classes that control the dynamic aspects of the app’s appearance.

## Utilities
This folder includes helper classes and utilities that provide common functionality across the application.
- Logging: Manages application logging for debugging, monitoring, and error tracking.
